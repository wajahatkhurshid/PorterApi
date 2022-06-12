using System;
using System.Collections.Generic;
using System.Text;

namespace Gyldendal.Porter.Common.Subscription
{
    public static class SubscriptionHelper
    {
        public static string GetSubscriptionName(bool isLocalDevelopment, string subscriptionName)
        {
            if (!isLocalDevelopment) return subscriptionName;

            var userName = Environment.UserName.ToLowerInvariant();
            return $"{subscriptionName}-{userName}";
        }
    }
}
