using Microsoft.EntityFrameworkCore;
using Talos.Server.Models;
using Talos.Server.Models.Entities;

namespace Talos.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ========== ENTIDADES PRINCIPALES ==========
        public DbSet<User> Users { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageVersion> PackageVersions { get; set; }
        public DbSet<PackageManager> PackageManagers { get; set; }
        public DbSet<TemplateDependencies> TemplateDependencies { get; set; }
        public DbSet<Compatibility> Compatibilities { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        //  NUEVAS ENTIDADES 
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserNotificationPreference> UserNotificationPreferences { get; set; }

    }


}