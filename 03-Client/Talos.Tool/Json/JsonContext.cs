// TemplateJsonContext.cs
using System.Text.Json.Serialization;
using Talos.Tool.Models;

namespace Talos.Tool.Json;

[JsonSerializable(typeof(TemplateJson))]
[JsonSerializable(typeof(List<TemplateJson>))]
[JsonSerializable(typeof(Dependency))]
[JsonSerializable(typeof(List<Dependency>))]
[JsonSerializable(typeof(Dictionary<string, List<string>>))]
public partial class TemplateJsonContext : JsonSerializerContext
{
}