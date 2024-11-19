using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var ui = builder.AddDockerfile("ui", "../../../_resources/ui")
        .WithHttpEndpoint(targetPort: 80);

builder.AddProject<Projects.WebDevMasterClass_Web>("web", "aspire")
        .WithExternalHttpEndpoints()
        .WithReference(ui.GetEndpoint("http"));

builder.AddProject<Projects.WebDevMasterClass_Services_Products>("products")
        .WithEnvironment("ConnectionStrings__Sql", builder.Configuration.GetConnectionString("Products"));

builder.Build().Run();
