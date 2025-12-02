using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Talos.Shared.Models;

public class template
{
    public  int id { get; set; }
    
    public int user_id { get; set; }
    public user User { get; set; } 
    
    public string template_name { get; set; }
    public string slug { get; set; }
    public bool is_public { get; set; }
    public string license_type { get; set; }
}