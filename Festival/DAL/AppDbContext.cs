using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Song> Songs { get; set; } = default!;
    public DbSet<Set> Sets { get; set; } = default!;
    public DbSet<SetSong> SetSongs { get; set; } = default!;
    public DbSet<Dj> Djs { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}