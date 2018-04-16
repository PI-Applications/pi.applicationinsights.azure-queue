using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PI.ApplicationInsights.AzureQueue
{
    public sealed class AzureQueueMetric : IDisposable
    {
        private bool _isDisposed = false;
        private TelemetryClient _telemetryClient { get; set; }
        private CloudQueueClient _queueClient { get; set; }
        private string _metricPrefix { get; set; }

        public int PushInterval { get; set; }

        public List<string> Queues { get; set; }
        
        public AzureQueueMetric(AzureQueueMetricOptions options)
        {
            _metricPrefix = options.MetricPrefix;
            _telemetryClient = options.TelemetryClient;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(options.StorageConnectionString);
            _queueClient = storageAccount.CreateCloudQueueClient();

            PushInterval = options.PushInterval ?? 60000;

            Queues = new List<string>();
            options.Queues.ForEach(x =>
            {
                Queues.Add(x);
                Queues.Add(x + "-poison");
            });

            Task.Run(MetricLoopAsync);
        }

        private async Task MetricLoopAsync()
        {
            while (!_isDisposed)
            {
                try
                {
                    foreach (var queue in Queues)
                    {
                        // Get queue
                        var cloudQueue = _queueClient.GetQueueReference(queue);
                        if (await cloudQueue.ExistsAsync())
                        {
                            // Fetch attributes
                            await cloudQueue.FetchAttributesAsync();

                            // Use ApproximateMessageCount as metric
                            var telemetryEnqueued = new MetricTelemetry(_metricPrefix + queue, cloudQueue.ApproximateMessageCount ?? 0);
                            _telemetryClient.TrackMetric(telemetryEnqueued);
                        }
                    }
                    
                    // Wait for next push
                    await Task.Delay(PushInterval).ConfigureAwait(continueOnCapturedContext: false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                    // Wait for next push
                    await Task.Delay(PushInterval).ConfigureAwait(continueOnCapturedContext: false);
                }
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
        }

        public static AzureQueueMetric Use(AzureQueueMetricOptions options)
        {
            return new AzureQueueMetric(options);
        }
    }
}
