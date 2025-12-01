namespace Talos.Shared.Models;

public class Compatibility
{
    public int id { get; set; }

    public int source_package_version_id { get; set; }
    public PackageVersion SourcePackageVersion { get; set; }

    public int target_package_id { get; set; }
    public Package TargetPackage { get; set; }

    public string target_version_constraint { get; set; }

    public compatibility_type compatibility_type { get; set; }
    public int compatibility_score { get; set; }

    public confidence_level confidence_level { get; set; }
    public detected_by detected_by { get; set; }

    public DateTime detection_date { get; set; }
    public string notes { get; set; }
    public bool is_active { get; set; }
}

public enum compatibility_type
{
    required,
    recommended,
    optional,
    conflict
}

public enum confidence_level
{
    high,
    medium,
    low
}

public enum detected_by
{
    n8n_scraper,
    manual,
    user_report,
    ai_analysis
}