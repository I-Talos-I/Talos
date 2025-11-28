namespace Talos.Shared.Models;

public class user
{
    public int id { get; set; }
    
    public string user_name { get; set; }
    
    
    public string email { get; set; }
    public string tier { get; set; } = "free";
    public int private_template_limit { get; set; } = 2;

    public DateTime createAt { get; set; } = DateTime.UtcNow;
    
    
}