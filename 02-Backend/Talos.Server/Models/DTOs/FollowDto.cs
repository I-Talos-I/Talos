namespace Talos.Server.Models.Dtos;

public class FollowDto
{
    public int Following_User_Id { get; set; }
    public int Followed_User_Id { get; set; }
    public DateTime Created_At { get; set; }
}