namespace Talos.Shared.Models;

public class TemplateDependencies
{
    public int id { get; set; }

    public int template_id { get; set; }
    public Template Template { get; set; }

    public int package_id { get; set; }
    public Package Package { get; set; }

    public string version_constraint { get; set; }
    public DateTime create_at { get; set; }
}