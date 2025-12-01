namespace Talos.Shared.Models;

public class Template
{
    public int id { get; set; }

    public int user_id { get; set; }
    public User User { get; set; }

    public string template_name { get; set; }
    public string slug { get; set; }
    public bool is_public { get; set; }
    public string license_type { get; set; }
    public DateTime create_at { get; set; }

    public ICollection<TemplateDependencies> dependencies { get; set; }
}