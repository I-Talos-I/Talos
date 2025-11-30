using Talos.Tool.Interfaces;
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
        // TODO: Implementar llamada real al backend
        await Task.Delay(100);
        
        return new TemplateJson
        {
            Name = "React Starter",
            Description = "A basic React template with TypeScript",
            Author = "talos-team",
            Dependencies = new List<Dependency>
            {
                new()
                {
                    Name = "node.js",
                    AvailableVersions = new List<string> { "18.x", "20.x", "22.x" },
                    Commands = new Dictionary<string, string[]>
                    {
                        ["linux"] = new[] { "curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -", "sudo apt-get install -y nodejs" },
                        ["windows"] = new[] { "choco install nodejs --version=20.0.0" }
                    }
                }
            }
        };
    }

    public async Task<List<CompatibilityResult>?> GetCompatibleVersionsAsync(string package, string version)
    {
        // TODO: Implementar llamada real al endpoint de compatibilidades
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