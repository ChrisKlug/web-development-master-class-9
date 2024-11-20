using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var idsrv = builder.AddContainer("identityserver", "identity-server")
                    .WithHttpsEndpoint(targetPort: 8081)
                    .WithEnvironment("ASPNETCORE_URLS", "https://*:8081")
                    .WithBindMount(Path.Combine(Environment.CurrentDirectory, "../../../_resources"), "/devcert", true)
                    .WithEnvironment("Kestrel__Certificates__Default__Path", "/devcert/ssl-cert.pfx")
                    .WithEnvironment("Kestrel__Certificates__Default__Password", "P@ssw0rd123!")
                    .WithExternalHttpEndpoints();

var ui = builder.AddDockerfile("ui", "../../../_resources/ui")
                    .WithHttpEndpoint(targetPort: 80);

var products = builder.AddProject<Projects.WebDevMasterClass_Services_Products>("products")
                        .WithEnvironment("ConnectionStrings__Sql", builder.Configuration.GetConnectionString("Products"));

builder.AddProject<Projects.WebDevMasterClass_Web>("web", "aspire")
        .WithExternalHttpEndpoints()
        .WithReference(ui.GetEndpoint("http"))
        .WithReference(products)
        .WithEnvironment("IdentityServer__Url", idsrv.GetEndpoint("https"))
        .WithHttpEndpoint(env: "DashboardPort");


builder.Build().Run();
