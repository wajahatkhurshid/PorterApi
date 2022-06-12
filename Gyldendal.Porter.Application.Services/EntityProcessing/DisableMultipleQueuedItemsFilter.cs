using System;
using System.Collections.Generic;
using System.Globalization;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.Storage;

namespace Gyldendal.Porter.Application.Services.EntityProcessing
{
    /// <summary>
    /// Completely stolen from: https://gist.github.com/odinserj/a8332a3f486773baa009
    /// This filter adds a fingerprint to jobs by generating a unique key based on the job type, method name and parameters.
    /// If the given fingerprint already exists on a job in the database, we will cancel the queueuing of the job, because the exact same process is already lined up to happen. 
    /// As an example, this will prevent enqueueing a product cooking for product ID 1234, if said product 1234 has already been put on the queue.
    /// However, if product 5678 gets queued up, it'll still go on the queue, because it has different parameters.
    /// This has been done simply to avoid an explosion of queued jobs during bootstrapping of product data. In day-to-day operations this filter should not make a difference.
    /// I think, if a processing fails and needs to retry, the fingerprint will still be removed, so the same job can still be queued up twice in that case, but it should not cause any problems either way.
    /// </summary>
    public class DisableMultipleQueuedItemsFilter : JobFilterAttribute, IClientFilter, IServerFilter
    {
        private static readonly TimeSpan LockTimeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan FingerprintTimeout = TimeSpan.FromHours(1);

        public void OnCreating(CreatingContext filterContext)
        {
            // If the fingerprint already exists, cancel the enqueueing, because the entity is already getting processed
            if (!AddFingerprintIfNotExists(filterContext.Connection, filterContext.Job))
            {
                filterContext.Canceled = true;
            }
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            RemoveFingerprint(filterContext.Connection, filterContext.Job);
        }
        
        private static bool AddFingerprintIfNotExists(IStorageConnection connection, Job job)
        {
            using (connection.AcquireDistributedLock(GetFingerprintLockKey(job), LockTimeout))
            {
                var fingerprint = connection.GetAllEntriesFromHash(GetFingerprintKey(job));
                
                DateTimeOffset timestamp;

                if (fingerprint != null &&
                    fingerprint.ContainsKey("Timestamp") &&
                    DateTimeOffset.TryParse(fingerprint["Timestamp"], null, DateTimeStyles.RoundtripKind, out timestamp) &&
                    DateTimeOffset.UtcNow <= timestamp.Add(FingerprintTimeout))
                {
                    // Actual fingerprint found, returning.
                    return false;
                }

                // Fingerprint does not exist, it is invalid (no `Timestamp` key),
                // or it is not actual (timeout expired).
                connection.SetRangeInHash(GetFingerprintKey(job), new Dictionary<string, string>
            {
                { "Timestamp", DateTimeOffset.UtcNow.ToString("o") }
            });

                return true;
            }
        }

        private static void RemoveFingerprint(IStorageConnection connection, Job job)
        {
            using (connection.AcquireDistributedLock(GetFingerprintLockKey(job), LockTimeout))
            using (var transaction = connection.CreateWriteTransaction())
            {
                transaction.RemoveHash(GetFingerprintKey(job));
                transaction.Commit();
            }
        }

        private static string GetFingerprintLockKey(Job job)
        {
            return $"{GetFingerprintKey(job)}:lock";
        }

        private static string GetFingerprintKey(Job job)
        {
            return $"fingerprint:{GetFingerprint(job)}";
        }

        private static string GetFingerprint(Job job)
        {
            string parameters = string.Empty;
            if (job.Args != null)
            {
                parameters = string.Join(".", job.Args);
            }
            if (job.Type == null || job.Method == null)
            {
                return string.Empty;
            }
            var fingerprint = $"{job.Type.FullName}.{job.Method.Name}.{parameters}";

            return fingerprint;
        }

        void IClientFilter.OnCreated(CreatedContext filterContext)
        {
        }

        void IServerFilter.OnPerforming(PerformingContext filterContext)
        {
        }
    }
}
