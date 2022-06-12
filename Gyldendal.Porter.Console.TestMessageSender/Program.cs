using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Gyldendal.Porter.Console.TestMessageSender
{
    class Program
    {
        private const string ConnectionString = "Endpoint=sb://gyldendal-dtfkit-sufyan.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=JpP7FJLTuG83gZJUO+Hn+GpJVB3UV/TNi6GKM+Jvbgk=";
        private const string TopicName = "portertopic";
        private static ServiceBusClient _client;
        private static ServiceBusSender _sender;
        private const int NumOfMessages = 30;

        static async Task Main(string[] args)
        {
            _client = new ServiceBusClient(ConnectionString);
            _sender = _client.CreateSender(TopicName);

            System.Console.WriteLine($" Sending {NumOfMessages * 3} messages on Topic: {TopicName}");
            LineBreak();

            try
            {
                var productMessages = PushMessagesToTopic("ProductBusinessObject");
                var workMessages = PushMessagesToTopic("WorkBusinessObject");
                var merchandiseMessages = PushMessagesToTopic("MerchandiseBusinessObject");

                await productMessages;
                await workMessages;
                await merchandiseMessages;
            }
            finally
            {
                System.Console.WriteLine("  Cleaning up....");
                await _sender.DisposeAsync();
                await _client.DisposeAsync();
            }

            LineBreak();
            System.Console.WriteLine("  Sending messages complete...");
        }

        private static async Task PushMessagesToTopic(string container)
        {
            using var messageBatch = await _sender.CreateMessageBatchAsync();

            for (var i = 1; i <= NumOfMessages; i++)
            {
                var message = JsonConvert.SerializeObject(new GpmMessage { ContainerId = $"{i}", Container = container });
                if (!messageBatch.TryAddMessage(new ServiceBusMessage(message)))
                {
                    // if it is too large for the batch
                    System.Console.WriteLine($" SKIPPED-MESSAGE-{i}: The message {message} is too large to fit in the batch.");
                }
            }

            await _sender.SendMessagesAsync(messageBatch);
            System.Console.WriteLine($" {container} batch of {NumOfMessages} messages has been published to the topic: {TopicName}");
            LineBreak();
        }

        private static void LineBreak()
        {
            System.Console.WriteLine("------------------------------------------------------------------------------------------------------");
            System.Console.WriteLine();
        }
    }
}
