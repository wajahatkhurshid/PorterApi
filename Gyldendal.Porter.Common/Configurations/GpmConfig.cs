namespace Gyldendal.Porter.Common.Configurations
{
    public class GpmConfig
    {
        public string GpmUrl { get; set; }

        public string GpmApiKey { get; set; }

        public string ServiceBusClient { get; set; }

        public string ServiceBusConnectionString { get; set; }

        public string TopicName { get; set; }

        public string SubscriptionName { get; set; }

        public bool IsLocalDevelopment { get; set; }
    }
}
