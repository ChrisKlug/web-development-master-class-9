var builder = DistributedApplication.CreateBuilder(args);

var ui = builder.AddDockerfile("ui", "../../../_resources/ui")
        .WithHttpEndpoint(targetPort: 80);

builder.AddProject<Projects.WebDevMasterClass_Web>("web", "aspire")
        .WithExternalHttpEndpoints()
        .WithReference(ui.GetEndpoint("http"));

builder.Build().Run();
