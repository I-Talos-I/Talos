namespace Talos.Server.Models.Dtos;

public class PackageDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Short_Name { get; set; }
    public int Package_Manager_Id { get; set; }
    public string Repository_Url { get; set; }
    public string Official_Documentation_Url { get; set; }
    public DateTime Last_Scraped_At { get; set; }
    public bool Is_Active { get; set; }
    public DateTime Create_At { get; set; }
    public DateTime Update_At { get; set; }
}