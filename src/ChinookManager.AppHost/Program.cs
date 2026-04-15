var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API>("api");
builder.AddProject<Projects.Presentation>("presentation");

builder.Build().Run();

