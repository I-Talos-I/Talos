namespace Talos.Server.Models.Dtos;

public class CompatibilityDto
{
    public int Id { get; set; }
    public int Source_Package_Version_Id { get; set; }
    public int Target_Package_Version_Id { get; set; }

    public string Target_Version_Constraint { get; set; }
    public int Compatibility_Score { get; set; }
    public string Compatibility_Type { get; set; }
    public string Confidence_Level { get; set; }
    public string Detected_By { get; set; }

    public DateTime Detection_Date { get; set; }
    public string Notes { get; set; }
    public bool Is_Active { get; set; }
}