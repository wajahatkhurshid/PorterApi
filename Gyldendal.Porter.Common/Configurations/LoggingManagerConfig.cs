using LoggingManager.Entities;

namespace Gyldendal.Porter.Common.Configurations
{
    public class LoggingManagerConfig : ILoggingManagerConfig
    {
        public string RabbitMqLogUser { get; set; }

        public string RabbitMqLogUserPassword { get; set; }

        public string RabbitMqLogHost { get; set; }

        public string RabbitMqLogVirtualHost { get; set; }

        public string RabbitMqLogExchange { get; set; }

        public int LogEventHrs { get; set; }

        public bool LogInfo { get; set; }

        public string LogName { get; set; }

        public bool MailExToInfra { get; set; }

        public string SourceName { get; set; }

        public string LogDirectoryPath { get; set; }

        public bool EnableEventLog { get; set; }

        public bool EnableDebugLog { get; set; }
    }
}
