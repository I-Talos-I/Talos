namespace Talos.Server.Models.Dtos;

public class TemplateDependencyDto
{
    public int Id { get; set; }
    public int Template_Id { get; set; }
    public int Package_Id { get; set; }
    public string Version_Constraint { get; set; }
    public DateTime Create_At { get; set; }
}