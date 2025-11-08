using System.Diagnostics.Metrics;

namespace WebDevMasterClass.Services.Orders.Observability;

public class OrdersMetrics
{
    public const string MeterName = "WebDevMasterClass.Services.Orders";
    private Counter<int> TotalOrdersCounter { get; }

    public OrdersMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        TotalOrdersCounter = meter.CreateCounter<int>("orders", "Order", "Orders added");
    }

    public void AddOrder() => TotalOrdersCounter.Add(1);
}