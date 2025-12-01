namespace Talos.Shared.Models;

public class PackageVersion
{
    public int id { get; set; }

    public int package_id { get; set; }
    public Package Package { get; set; }

    public string version { get; set; }
    public DateTime? release_date { get; set; }
    public bool is_deprecated { get; set; }
    public string deprecation_message { get; set; }
    public string download_url { get; set; }
    public string release_notes_url { get; set; }

    public DateTime create_at { get; set; }

    public ICollection<Compatibility> compatibilities_source { get; set; }
}