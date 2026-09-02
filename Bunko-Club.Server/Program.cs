using Bunko_Club.Server;
using Bunko_Club_Server;
using Scalar.AspNetCore;
using FastEndpoints;

var builder = WebApplication.CreateSlimBuilder(args);
builder.WebHost.UseKestrelHttpsConfiguration();

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.Services
  .AddFastEndpoints(DiscoveredTypes.All)
  .AddOpenApi();

builder.Services.ConfigureHttpJsonOptions(static options =>
{
  options.SerializerOptions.AddSerializerContextsFromBunko_Club_Server();
});

// Add services to the container.
builder.Services.AddProblemDetails();

using var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();
app.UseFastEndpoints(static c =>
    {
      c.Serializer.Options.AddSerializerContextsFromBunko_Club_Server();
      c.Binding.ReflectionCache.AddFromBunkoClubServer();
    });

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

app.UseFileServer();

await app.RunAsync();
