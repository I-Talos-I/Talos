namespace Talos.Shared.Models;

public class Follow
{
    public int id { get; set; }
    public int following_user_id { get; set; }
    public User FollowingUser { get; set; }

    public int followed_user_id { get; set; }
    public User FollowedUser { get; set; }

    public DateTime created_at { get; set; } = DateTime.UtcNow;
}