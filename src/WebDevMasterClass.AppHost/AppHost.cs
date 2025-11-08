using Projects;

var builder = DistributedApplication.CreateBuilder(args);

#pragma warning disable ASPIREINTERACTION001
var sslPwdParameter = builder.AddParameter("ssl-password", true)
    .WithCustomInput(parameter => new()
    {
        Name = parameter.Name,
        Label = "SSL Certificate Password",
        Placeholder = "The password",
        InputType = InputType.SecretText,
        Required = true
    });
#pragma warning restore ASPIREINTERACTION001

var idsrv = builder.AddContainer("identityserver", "zerokoll/webdevworkshop-identity-server")
    .WithHttpsEndpoint(targetPort: 8081)
    .WithEnvironment("ASPNETCORE_URLS", "https://*:8081")
    .WithBindMount(
        Path.Combine(Environment.CurrentDirectory, "../../ssl-cert.pfx"), 
        "/devcert/ssl-cert.pfx", 
        true)
    .WithEnvironment("Kestrel__Certificates__Default__Path", "/devcert/ssl-cert.pfx")
    .WithEnvironment("Kestrel__Certificates__Default__Password", sslPwdParameter)
    .WithExternalHttpEndpoints();

sslPwdParameter.WithParentRelationship(idsrv);

var sql = builder.AddSqlServer("sqlserver")
    .WithDataVolume("webdevdata")
    .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("WebDevMasterClass-Products");
var ordersDb = sql.AddDatabase("WebDevMasterClass-Orders");

var ui = builder.AddDockerfile("ui", "../../_resources/ui")
    .WithHttpEndpoint(targetPort: 80)
    .WithHttpHealthCheck("/");

var products = builder.AddProject<WebDevMasterClass_Services_Products>("products", "https")
    .WithReference(db)
    .WaitFor(db);

var orders = builder.AddProject<WebDevMasterClass_Services_Orders>("orders", "https")
    .WithEnvironment("ConnectionStrings__Sql", ordersDb.Resource.ConnectionStringExpression);

var web = builder.AddProject<WebDevMasterClass_Web>("web", "aspire")
    .WithExternalHttpEndpoints()
    .WithHttpEndpoint(name: "Dashboard", env: "DashboardPort")
    .WithUrlForEndpoint("Dashboard", url =>
    {
        url.DisplayText = "Orleans Dashboard";
    })
    .WithEnvironment("IdentityServer__Url", idsrv.GetEndpoint("https"))
    .WithReference(ui.GetEndpoint("http"))
    .WaitFor(ui)
    .WithReference(products)
    .WaitFor(products)
    .WithReference(orders)
    .WaitFor(orders)
    .WithHttpHealthCheck("/health");

builder.Build().Run();
