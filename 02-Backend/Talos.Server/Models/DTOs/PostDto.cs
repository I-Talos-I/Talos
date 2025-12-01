namespace Talos.Server.Models.Dtos;

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }

    public int User_Id { get; set; }
    public string Status { get; set; }
    public DateTime Created_At { get; set; }
}