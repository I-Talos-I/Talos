using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Talos.Server.Models.Entities;

namespace Talos.Server.Models;

[Table("Users")] // Especificar nombre de tabla explícitamente
public class User
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("Username")]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    [Column("Email")]
    public string Email { get; set; }  

    [Required]
    [Column("PasswordHash")]
    public string PasswordHash { get; set; }  
    [Column("AvatarUrl")]
    public string AvatarUrl { get; set; }
    [Required]
    public string SignalConnectionID  { get; set; }
    
    public string LastConnectionID { get; set; }
    public bool IsOnline { get; set; }
    public DateTime LastSeenAt { get; set; }
    public string LastIpAddress { get; set; }
    public string Tier { get; set; }
    public int PrivateTemplateLimit { get; set; }
    public int CurrrentTemplateLimit { get; set; }
    public bool EmailVerified { get; set; }
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("Role")]
    public string Role { get; set; } = "user";


    // Propiedades de navegación
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();
    public ICollection<Follow> Following { get; set; } = new List<Follow>();
    public ICollection<Template> Templates { get; set; } = new List<Template>();
    // Propiedades de navegación adicionales 
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<UserNotificationPreference> NotificationPreferences { get; set; } = new List<UserNotificationPreference>();
}