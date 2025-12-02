namespace Talos.Shared.Models;

public class package
{
    public int id { get; set; }
    public  string name { get; set; }
    public string short_name { get; set; }
    public string package_manager { get; set; }
    public DateTime created_at { get; set; } = DateTime.UtcNow;
}