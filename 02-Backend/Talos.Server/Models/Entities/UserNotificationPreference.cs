using System.ComponentModel.DataAnnotations.Schema;

namespace Talos.Server.Models.Entities;

public class UserNotificationPreference
{
    public int Id { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    
    [ForeignKey("Tag")]
    public int TagId { get; set; }
    
    public bool ViaEmail { get; set; }
    public bool ViaPush { get; set; }
    public bool ViaWeb { get; set; }
    public bool IsMuted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
     
    // Propiedades de navegación
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>(); 
}