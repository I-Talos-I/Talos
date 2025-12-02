namespace Talos.Shared.Models;

public class package_version
{
    public int id { get; set; }
    
    public int package { get; set; }
    public package Package { get; set; }
    
    public string version { get; set; }
    public DateTime release_date { get; set; }
    
    public bool is_deprecated { get; set; }
}