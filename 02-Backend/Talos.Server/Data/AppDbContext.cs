using Microsoft.EntityFrameworkCore;
using Talos.Shared.Models;

namespace Talos.Shared.Data;

public class AppDbContext : DbContext
{

    public DbSet<user> Users { get; set; }
    public DbSet<template> Templates { get; set; }
    public DbSet<package> Packages { get; set; }
    public DbSet<package_version> PackageVersions { get; set; }
    public DbSet<template_denpendencies> TemplateDependencies { get; set; }
    public DbSet<compatibility> Compatibility { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext>options) :  base(options)
    {
        
    }
    
}