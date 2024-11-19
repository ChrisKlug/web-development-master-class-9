using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebDevMasterClass.Services.Products.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddFastEndpoints();

builder.AddSqlServerDbContext<ProductsContext>("Sql");
builder.Services.AddScoped<IProducts, EfProducts>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    using (var ctx = scope.ServiceProvider.GetRequiredService<ProductsContext>())
    {
        ctx.Database.Migrate();
        var sql = File.ReadAllText("Data/SeedData.sql");
        ctx.Database.ExecuteSqlRaw(sql);
    }
}

app.MapDefaultEndpoints();

app.UseFastEndpoints();

app.Run();

public partial class Program { }
