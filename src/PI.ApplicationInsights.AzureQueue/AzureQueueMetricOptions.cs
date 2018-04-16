using Microsoft.ApplicationInsights;
using System.Collections.Generic;

namespace PI.ApplicationInsights.AzureQueue
{
    public class AzureQueueMetricOptions
    {
        public string MetricPrefix { get; set; }
        public string StorageConnectionString { get; set; }
        public TelemetryClient TelemetryClient { get; set; }
        public int? PushInterval { get; set; }
        public List<string> Queues { get; set; }
    }
}
