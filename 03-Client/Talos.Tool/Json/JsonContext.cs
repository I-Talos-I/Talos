using System.Text.Json.Serialization;
using Talos.Tool.Models;

namespace Talos.Tool.Json;

[JsonSerializable(typeof(TemplateJson))]
[JsonSerializable(typeof(Dependency))]
[JsonSerializable(typeof(CompatibleDependency))]
[JsonSerializable(typeof(CompatibilityResult))]
public partial class TemplateJsonContext : JsonSerializerContext
{
}