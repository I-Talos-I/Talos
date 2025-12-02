using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using Talos.Tool.Interfaces;
using Talos.Tool.Json;
using Talos.Tool.Models;
using Talos.Tool.Utilities;

namespace Talos.Tool.Services;

public class TemplateService : ITemplateService
{
    private readonly HttpClient _httpClient;

    public TemplateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<TemplateJson?> GetTemplateAsync(string templateSlug)
    {
        try
        {
            AnsiConsole.MarkupLineInterpolated($"[grey]Downloading template: {templateSlug}[/]");

            var response = await _httpClient.GetAsync($"http://localhost:3000/r/{templateSlug}.json");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            
            AnsiConsole.MarkupLineInterpolated($"[grey]JSON received: {jsonString.Length} characters,[/]");

            var template = JsonHelper.Deserialize<TemplateJson>(jsonString);

            if (template is null)
            {
                AnsiConsole.MarkupLine("[red]Failed to deserialize template JSON.[/]");
                return null;
            }
            
            return template;
        }
        catch (HttpRequestException ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error parsing Template: {ex.Message}[/]");
            return null;
        }
        catch (JsonException ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error parsing JSON: {ex.Message}[/]");
            return null;
        }
    }

    public async Task<List<CompatibilityResult>?> GetCompatibleVersionsAsync(string package, string version)
    {
        await Task.Delay(50);

        return new List<CompatibilityResult>
        {
            new()
            {
                Package = "npm",
                CompatibleVersions = new List<string> { "10.x", "11.x" },
                CompatibilityScore = 0.95
            }
        };
    }
}