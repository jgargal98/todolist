using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data;

// Antes: public class AppDbContext : DbContext
public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<TodoTask> Tasks => Set<TodoTask>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Obligatorio para Identity

        modelBuilder.Entity<TodoTask>(entity =>
        {
            //
            entity.OwnsMany(t => t.SubTasks, builder =>
            {
                builder.ToJson();
            });

            // N:M - new table "task_tags"
            entity.HasMany(t => t.Tags)
                  .WithMany(tag => tag.Tasks)
                  .UsingEntity("task_tags");
        });
    }
}