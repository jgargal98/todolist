using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<TodoTask> Tasks => Set<TodoTask>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoTask>(entity =>
        {
            // SubTasks se guarda como JSON
            entity.OwnsMany(t => t.SubTasks, builder => { builder.ToJson(); });

            // Tabla intermedia task_tags
            entity.HasMany(t => t.Tags)
                  .WithMany(tag => tag.Tasks)
                  .UsingEntity("task_tags");
        });
    }
}