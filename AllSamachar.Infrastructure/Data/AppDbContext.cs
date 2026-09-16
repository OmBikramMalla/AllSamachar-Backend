using AllSamachar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AllSamachar.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<News> News => Set<News>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<User> Users => Set<User>();
    public DbSet<SavedStory> SavedStories => Set<SavedStory>();
    public DbSet<ReadingHistory> ReadingHistory => Set<ReadingHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Slug)
            .IsUnique();

        modelBuilder.Entity<Publisher>()
            .HasIndex(p => p.Slug)
            .IsUnique();

        modelBuilder.Entity<News>()
            .HasIndex(n => n.Slug)
            .IsUnique();

        modelBuilder.Entity<SavedStory>()
            .HasIndex(s => new { s.UserId, s.NewsId })
            .IsUnique();
    }
}