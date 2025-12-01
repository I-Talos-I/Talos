namespace Talos.Shared.Models;

public class Post
{
    public int id { get; set; }
    public string title { get; set; }
    public string body { get; set; }
    public int user_id { get; set; }
    public User User { get; set; }
    public string status { get; set; }
    public DateTime created_at { get; set; } = DateTime.UtcNow;
}