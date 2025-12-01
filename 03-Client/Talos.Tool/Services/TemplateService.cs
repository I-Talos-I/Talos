using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Talos.Tool.Interfaces;
using Talos.Tool.Json;
using Talos.Tool.Models;

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
        var filePath = Path.Combine("/home/sergio/Documentos/talos/03-Client/Talos.Tool/Schemas", $"{templateSlug}.json");

        Console.WriteLine($"[TEMPLATE DEBUG] Path: {filePath}");
        Console.WriteLine($"[TEMPLATE DEBUG] Exists: {File.Exists(filePath)}");
        
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"No existe el template: {filePath}");

        await using var fileStream = File.OpenRead(filePath);

        return await JsonSerializer.DeserializeAsync(
            fileStream,
            TemplateJsonContext.Default.TemplateJson
        );
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