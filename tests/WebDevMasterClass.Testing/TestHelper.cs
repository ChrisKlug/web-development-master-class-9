using System.Data.Common;
using System.Net.Http.Headers;
using System.Text;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebDevMasterClass.Testing;

public static class TestHelper
{
    public static async Task ExecuteTest<TProgram, TDbContext>(
        Func<DbCommand, Task> dbSetup,
        Func<HttpClient, Task> test)
        where TProgram : class
        where TDbContext : DbContext
    {
        var app = new WebApplicationFactory<TProgram>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("IntegrationTesting");
                // builder.ConfigureAppConfiguration((ctx, config) =>
                // {
                //     config.AddInMemoryCollection([
                //         new KeyValuePair<string, string?>("ConnectionStrings:WebDevMasterClass-Products",
                //             "Server=.;Database=WebDevMasterClass.Products.Tests;User Id=sa;Password=MyVerySecretPassw0rd;Encrypt=Yes;Trust Server Certificate=Yes;")
                //     ]);
                // });
                builder.ConfigureTestServices(services =>
                {
                    var dbDescriptor = services.First(x => x.ServiceType == typeof(TDbContext));
                    var optionsDescriptor =
                        services.First(x => x.ServiceType == typeof(DbContextOptions<TDbContext>));

                    services.Remove(dbDescriptor);
                    services.Remove(optionsDescriptor);

                    services.AddDbContext<TDbContext>((svc, options) =>
                    {
                        var config = svc.GetRequiredService<IConfiguration>();
                        options.UseSqlServer(config.GetConnectionString("WebDevMasterClass-Products"),
                            opts => { opts.ExecutionStrategy(x => new NonRetryingExecutionStrategy(x)); });
                    }, ServiceLifetime.Singleton);
                    
                    services.AddTestAuthentication();
                });
            });

        var ctx = app.Services.GetRequiredService<TDbContext>();

        await using var transaction = await ctx.Database.BeginTransactionAsync();
        await using var conn = ctx.Database.GetDbConnection();

        var cmd = conn.CreateCommand();
        cmd.Transaction = transaction.GetDbTransaction();

        await dbSetup(cmd);

        var client = app.CreateClient();

        await test(client);
    }
    
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
    
    public static async Task ExecuteTest<TProgram, TDbContext, TGrpcService>(
        Func<TGrpcService, Task> test,
        Func<DbCommand, Task>? dbConfig = null,
        Func<DbCommand, Task>? validateDb = null
    )
        where TProgram : class
        where TDbContext : DbContext
        where TGrpcService : ClientBase
    {
        var app = new WebApplicationFactory<TProgram>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("IntegrationTesting");

                builder.ConfigureTestServices(services =>
                {
                    var dbDescriptor = services.First(x => x.ServiceType == typeof(TDbContext));
                    var optionsDescriptor = services.First(x => x.ServiceType == typeof(DbContextOptions<TDbContext>));

                    services.Remove(dbDescriptor);
                    services.Remove(optionsDescriptor);

                    var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

                    services.AddDbContext<TDbContext>((svc, options) =>
                    {
                        options.UseSqlServer(config.GetConnectionString("Sql"), opts =>
                        {
                            opts.ExecutionStrategy(x => new NonRetryingExecutionStrategy(x));
                        });
                    }, ServiceLifetime.Singleton);
                });
            });

        using var services = app.Services.CreateScope();

        var ctx = services.ServiceProvider.GetRequiredService<TDbContext>();

        await using var transaction = ctx.Database.BeginTransaction();

        var conn = ctx.Database.GetDbConnection();
        var cmd = conn.CreateCommand();
        cmd.Transaction = transaction.GetDbTransaction();

        if (dbConfig != null)
            await dbConfig(cmd);

        var options = new GrpcChannelOptions {
            HttpHandler = app.Server.CreateHandler()
        };
        var channel = GrpcChannel.ForAddress(app.Server.BaseAddress, options);
        var client = (TGrpcService)Activator.CreateInstance(typeof(TGrpcService), channel)!;
        
        await test(client);

        if (validateDb != null)
            await validateDb(cmd);
    }
}