using Microsoft.EntityFrameworkCore;
using Talos.Shared.Models;

namespace Talos.Shared.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    public DbSet<User> users { get; set; }
    public DbSet<Template> templates { get; set; }
    public DbSet<Package> packages { get; set; }
    public DbSet<PackageVersion> package_versions { get; set; }
    public DbSet<PackageManager> package_managers { get; set; }
    public DbSet<TemplateDependencies> template_dependencies { get; set; }
    public DbSet<Compatibility> compatibilities { get; set; }
    public DbSet<Follow> follows { get; set; }
    public DbSet<Post> posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Template>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.user_id);

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.FollowingUser)
            .WithMany()
            .HasForeignKey(f => f.following_user_id);

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.FollowedUser)
            .WithMany()
            .HasForeignKey(f => f.followed_user_id);

        modelBuilder.Entity<TemplateDependencies>()
            .HasOne(td => td.Template)
            .WithMany()
            .HasForeignKey(td => td.template_id);

        modelBuilder.Entity<TemplateDependencies>()
            .HasOne(td => td.Package)
            .WithMany()
            .HasForeignKey(td => td.package_id);
    }


  
}