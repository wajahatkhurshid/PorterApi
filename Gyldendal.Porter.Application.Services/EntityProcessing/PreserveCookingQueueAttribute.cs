using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace Gyldendal.Porter.Application.Services.EntityProcessing
{
    public class PreserveCookingQueueAttribute : JobFilterAttribute, IElectStateFilter, IApplyStateFilter
    {
        private readonly string _cookingQueue;


        public PreserveCookingQueueAttribute(string queue)
        {
            _cookingQueue = queue;
        }
        public void OnStateElection(ElectStateContext context)
        {
            if (context.CandidateState is EnqueuedState enqueueState)
            {
                enqueueState.Queue = _cookingQueue;
            }
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            // Activating only when enqueue a background job
            if (context.NewState is EnqueuedState enqueueState)
            {
                // Checking if an original queue is already set
                var originalQueue = SerializationHelper.Serialize(context.Connection.GetJobParameter(context.BackgroundJob.Id, _cookingQueue));

                if (originalQueue != null)
                {
                    // Override any other queue value that is currently set (by other filters, for example)
                    enqueueState.Queue = _cookingQueue;
                }
                else
                {
                    // Queueing for the first time, we should set the original queue
                    context.Connection.SetJobParameter(context.BackgroundJob.Id, _cookingQueue, SerializationHelper.Serialize(enqueueState.Queue));
                }
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}