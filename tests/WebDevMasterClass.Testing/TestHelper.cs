using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;

namespace WebDevMasterClass.Testing;

public static class TestHelper
{
    public static async Task ExecuteTest<TProgram>(
        Func<HttpClient, Task> test,
        Action<IServiceCollection>? serviceConfig = null,
        bool isAuthenticated = true
    )
        where TProgram : class
    {
        var app = new WebApplicationFactory<TProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("IntegrationTesting");

                    builder.ConfigureTestServices(services =>
                    {
                        serviceConfig?.Invoke(services);

                        services.AddTestAuthentication();
                    });
                });

        var client = app.CreateClient();

        if (isAuthenticated)
        {
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes("test:test"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        }

        await test(client);
    }

    public static async Task ExecuteTest<TProgram, TDbContext>(
        Func<SqlCommand, Task> dbSetup,
        Func<HttpClient, Task> test,
        bool isAuthenticated = true
    )
        where TProgram : class
        where TDbContext : DbContext
    {
        var app = new WebApplicationFactory<TProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("IntegrationTesting");
                    //builder.ConfigureAppConfiguration((ctx, config) =>
                    //{
                    //    config.AddInMemoryCollection([
                    //        new KeyValuePair<string, string?>("ConnectionStrings:Sql", "Server=.;Database=WebDevMasterClass.Products.Tests;User Id=sa;Password=MyVerySecretPassw0rd;Encrypt=Yes;Trust Server Certificate=Yes;")
                    //    ]);
                    //});

                    builder.ConfigureTestServices(services =>
                    {
                        var dbDescriptor = services.First(x => x.ServiceType == typeof(TDbContext));
                        var optionsDescriptor = services.First(x => x.ServiceType == typeof(DbContextOptions<TDbContext>));

                        services.Remove(dbDescriptor);
                        services.Remove(optionsDescriptor);

                        services.AddDbContext<TDbContext>((services, options) =>
                        {
                            var config = services.GetRequiredService<IConfiguration>();
                            options.UseSqlServer(config.GetConnectionString("Sql"), options =>
                            {
                                options.ExecutionStrategy(x => new NonRetryingExecutionStrategy(x));
                            });
                        }, ServiceLifetime.Singleton);

                        services.AddTestAuthentication();
                    });
                });

        var ctx = app.Services.GetRequiredService<TDbContext>();

        using (var transaction = ctx.Database.BeginTransaction())
        using (var conn = ctx.Database.GetDbConnection())
        {
            var cmd = (SqlCommand)conn.CreateCommand();
            cmd.Transaction = (SqlTransaction)transaction.GetDbTransaction();

            await dbSetup(cmd);

            var client = app.CreateClient();

            if (isAuthenticated)
            {
                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes("test:test"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            }

            await test(client);
        }
    }
}
