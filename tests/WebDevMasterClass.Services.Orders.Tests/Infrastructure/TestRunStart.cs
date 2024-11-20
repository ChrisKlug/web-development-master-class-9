extern alias SERVER;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using Xunit.Sdk;
using OrdersServer = SERVER::WebDevMasterClass.Services.Orders;

[assembly: TestFramework("WebDevMasterClass.Services.Orders.Tests.Infrastructure.TestRunStart", "WebDevMasterClass.Services.Orders.Tests")]

namespace WebDevMasterClass.Services.Orders.Tests.Infrastructure;

public class TestRunStart : XunitTestFramework
{
    public TestRunStart(IMessageSink messageSink) : base(messageSink)
    {
        var config = new ConfigurationManager()
                    .AddJsonFile("appSettings.IntegrationTesting.json")
                    .Build();

        var options = new DbContextOptionsBuilder<OrdersServer.Data.OrdersContext>()
                        .UseSqlServer(config.GetConnectionString("Sql"))
                        .ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning));

        var dbContext = new OrdersServer.Data.OrdersContext(options.Options);

        dbContext.Database.Migrate();
    }
}
