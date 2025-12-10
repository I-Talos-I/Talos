using System.ComponentModel.DataAnnotations.Schema;

namespace Talos.Server.Models.Entities;

public class Notification
{
    public int Id { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    
    [ForeignKey("Tag")]
    public int TagId { get; set; }
    
    public string Title { get; set; }
    public string Payload { get; set; }
    public bool IsRead { get; set; }
    public bool IsArchived { get; set; }
    public bool Priority { get; set; }
    public DateTime ExpiersAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ArchivedAt { get; set; }
    
    // Propiedades de navegación
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>(); 
}