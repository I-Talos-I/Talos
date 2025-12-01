namespace Talos.Server.Models.Dtos;

public class PackageVersionDto
{
    public int Id { get; set; }
    public int Package_Id { get; set; }
    public string Version { get; set; }
    public DateTime Release_Date { get; set; }
    public bool Is_Deprecated { get; set; }
    public string Deprecation_Message { get; set; }
    public string Download_Url { get; set; }
    public string Release_Notes_Url { get; set; }
    public DateTime Create_At { get; set; }
}