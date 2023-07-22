using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Threading.Tasks;

namespace IOTHubIngestionSignalR
{
    public class IOTSignalRFunction
    {
        private static HttpClient client = new HttpClient();
        
        [FunctionName("IOTSignalRFunction")]
        public static async Task RunAsync(
            [IoTHubTrigger("messages/events", Connection = "IoTHubEndpoint")]EventData message,
            [SignalR(HubName = "IOTSampleData")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {

            var iOTDeviceMessage = Encoding.UTF8.GetString(message.Body.Array);
            log.LogInformation($"C# IoT Hub trigger function processed a message: {iOTDeviceMessage}");

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "iotDeviceMessage",
                    Arguments = new[] { iOTDeviceMessage }
                })
                .ConfigureAwait(false);
        }
    }
}