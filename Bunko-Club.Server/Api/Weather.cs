using System.Collections.Immutable;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Api;

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
public record WeatherForecastResponse(ImmutableArray<WeatherForecast> Forecasts);

[HttpGet("/api/weatherforecast")]
[AllowAnonymous]
public sealed class WeatherEndpoint : EndpointWithoutRequest<WeatherForecastResponse>
{
  static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];
  public override async Task HandleAsync(CancellationToken ct)
  {
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            Summaries[Random.Shared.Next(Summaries.Length)]
        ))
        .ToImmutableArray();
    await Send.OkAsync(new(forecast), ct);
  }
}
