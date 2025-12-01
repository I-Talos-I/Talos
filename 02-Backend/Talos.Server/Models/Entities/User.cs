namespace Talos.Shared.Models;

public class User
{
    public int id { get; set; }
    public string user_name { get; set; }
    public string email { get; set; }
    public string tier { get; set; }
    public int private_template_limit { get; set; }
    public DateTime create_at { get; set; }

    public ICollection<Template> templates { get; set; }
}