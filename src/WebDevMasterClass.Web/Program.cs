using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpForwarderWithServiceDiscovery();
builder.Services.AddMvc();

builder.Services.AddProductsClient(options =>
{
    options.BaseAddress = "https://products";
});

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = "/images/products",
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "../../_resources/productimages"))
});

app.UseRouting();

app.MapDefaultEndpoints();

app.MapControllers();

app.Map("/api/{**catch-all}", (HttpContext ctx) => {
    ctx.Response.StatusCode = 404;
});

app.MapForwarder("/{**catch-all}", "https+http://ui");

app.Run();
