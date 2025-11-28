namespace Talos.Shared.Models;

public class template_denpendencies
{
    public int id { get; set; }
    
    public int template_id { get; set; }
    public template Template { get; set; }
    
    public int package_id { get; set; }
    public package Package { get; set; }
    
    public string version_constraint { get; set; }
}