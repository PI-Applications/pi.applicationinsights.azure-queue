# PI.ApplicationInsights.AzureQueue

![Build status from VSTS](https://pi-applications-dk.visualstudio.com/_apis/public/build/definitions/8c43066a-ced2-41f9-822b-b5a7154a9b31/57/badge)
![NuGet](https://img.shields.io/nuget/v/PI.ApplicationInsights.AzureQueue.svg)
![NuGet Pre Release](https://img.shields.io/nuget/vpre/PI.ApplicationInsights.AzureQueue.svg)

Use this package if you want to monitor status for one or more queues in Azure Storage with Azure Application Insights. 

The following metrics will be pushed to Azure Application Insights:
- ApproximateMessageCount on primary queue
- ApproximateMessageCount on poison queue

## Example
```csharp
AzureQueueMetric.Use(new AzureQueueMetricOptions
{
    MetricPrefix = "MyWebshop",
    PushInterval = 120000,
    Queues = new List<string> { "new-orders", "capture-payments" },
    StorageConnectionString = "UseDevelopmentStorage=true",
    TelemetryClient = aiClient
});
```

## Options
Property | Required | Description
--- | --- | ---
Queues | **yes** | List of queues to be monitored. *-poison* queues will be added automatic.
StorageConnectionString | **yes** | Connection string to Azure Storage
TelemetryClient | **yes** | Application Insights instance
MetricPrefix  | **yes** | Prefix for metrics names
PushInterval | no | Push interval in milliseconds. Default is 60000 (every minute)

## Metrics
Name | Description
--- | ---
[*MetricPrefix*]-[*QueueName*] | Approximate message count for primary queue
[*MetricPrefix*]-[*QueueName*]-poison | Approximate message count for poison queue

### Example of metrics
If you add two queues named **new-orders** and **capture-payments**, and use this package with a prefix called **MyWebshop** then you will get the following metrics:
- *MyWebshop*-**new-orders**
- *MyWebshop*-**new-orders**-poison
- *MyWebshop*-**capture-payments**
- *MyWebshop*-**capture-payments**-poison