using Microsoft.EntityFrameworkCore;
using WebDevMasterClass.Services.Orders.Data;
using WebDevMasterClass.Services.Orders.Data.Interceptors;
using WebDevMasterClass.Services.Orders.Observability;
using WebDevMasterClass.Services.Orders.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSqlServerDbContext<OrdersContext>("Sql", configureDbContextOptions: options => {
    options.AddInterceptors(OrderCreatedInterceptor.Instance);
});

builder.Services.AddGrpc();

builder.Services.AddSingleton<OrdersMetrics>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
        tracing.AddSource(OrdersService.ActivitySourceName)
    )
    .WithMetrics(options => 
        options.AddMeter(OrdersMetrics.MeterName)
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    using var ctx = scope.ServiceProvider.GetRequiredService<OrdersContext>();
    
    ctx.Database.Migrate();
}

app.MapGrpcService<OrdersService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapDefaultEndpoints();

app.Run();

public partial class Program {}