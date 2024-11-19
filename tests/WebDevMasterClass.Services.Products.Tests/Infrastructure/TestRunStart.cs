using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebDevMasterClass.Services.Products.Data;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: TestFramework(
    "WebDevMasterClass.Services.Products.Tests.Infrastructure.TestRunStart",
    "WebDevMasterClass.Services.Products.Tests"
)]

namespace WebDevMasterClass.Services.Products.Tests.Infrastructure;

public class TestRunStart : XunitTestFramework
{
    public TestRunStart(IMessageSink messageSink) : base(messageSink)
    {
        var config = new ConfigurationManager()
                        .AddJsonFile("appSettings.IntegrationTesting.json")
                        .Build();

        var options = new DbContextOptionsBuilder<ProductsContext>()
                            .UseSqlServer(config.GetConnectionString("Sql"));

        var dbContext = new ProductsContext(options.Options);

        dbContext.Database.Migrate();
    }
}
