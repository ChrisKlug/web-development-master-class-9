var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerfile("ui", "../../../_resources/ui")
        .WithHttpEndpoint(targetPort: 80);

builder.Build().Run();
