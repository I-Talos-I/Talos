namespace Talos.Shared.Models;

public class Package
{
    public int id { get; set; }

    public string name { get; set; }
    public string short_name { get; set; }

    public int package_manager_id { get; set; }
    public PackageManager PackageManager { get; set; }

    public string repository_url { get; set; }
    public string official_documentation_url { get; set; }

    public DateTime? last_scraped_at { get; set; }
    public bool is_active { get; set; }

    public DateTime create_at { get; set; }
    public DateTime update_at { get; set; }

    public ICollection<PackageVersion> versions { get; set; }
    public ICollection<Compatibility> compatibilities_target { get; set; }
}