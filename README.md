# PI.ApplicationInsights.AzureQueue

![Build status from VSTS](https://pi-applications-dk.visualstudio.com/_apis/public/build/definitions/8c43066a-ced2-41f9-822b-b5a7154a9b31/57/badge)

Use this package if you want to monitor status for one or more queues in Azure Storage. 

The following metrics will be pushed to AI:
* ApproximateMessageCount on primary queue
* ApproximateMessageCount on poison queue

## Example
```
AzureQueueMetric.Use(new AzureQueueMetricOptions
{
    MetricPrefix = "myprefix",
    PushInterval = 120000,
    Queues = new List<string> { "orders", "payments" },
    StorageConnectionString = "localhost",
    TelemetryClient = aiClient
});
```

## Options
Property | Required | Description
--- | --- | ---
Queues | **yes** | List of queues to be monitored. *-poison* queues will be added automatic.
StorageConnectionString | **yes** | Connection string to Azure Storage
TelemetryClient | **yes** | Application Insights instance
MetricPrefix  | no | Prefix for metrics name
PushInterval | no | Push interval in milliseconds. Default is 60000 (every minute)