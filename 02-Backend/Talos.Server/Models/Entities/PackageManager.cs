namespace Talos.Shared.Models;

public class PackageManager
{
    public int id { get; set; }
    public string name { get; set; }

    public ICollection<Package> packages { get; set; }
}