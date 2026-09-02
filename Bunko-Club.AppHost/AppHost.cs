var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddProject<Projects.Bunko_Club_Server>("server")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints()
    .WithUrlForEndpoint("https", static url =>
    {
      url.DisplayText = "Scalar (HTTPS)";
      url.Url = "/scalar";
    });

var frontend = builder.AddJavaScriptApp("frontend", "../Bunko-Club.Frontend", runScriptName: "start")
    .WithNpm(installCommand: "ci")
    .WithReference(server)
    .WaitFor(server)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

server.PublishWithContainerFiles(frontend, "wwwroot");

builder.Build().Run();
