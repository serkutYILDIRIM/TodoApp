using Microsoft.EntityFrameworkCore;
using TodoApp.API.Models;

namespace TodoApp.API.Data;

public class TodoAppDbContext : DbContext
{
    public TodoAppDbContext(DbContextOptions<TodoAppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<Activity> Activities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).ValueGeneratedOnAdd();
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.CreatedDate).IsRequired();
        });

        // Configure TodoItem entity
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(e => e.TodoId);
            entity.Property(e => e.TodoId).ValueGeneratedOnAdd();
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.IsCompleted).IsRequired();
            entity.Property(e => e.Detail).HasMaxLength(int.MaxValue);
            entity.Property(e => e.Priority).HasMaxLength(20);

            // Configure relationship with User
            entity.HasOne(e => e.User)
                .WithMany(u => u.TodoItems)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Activity entity
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivityId);
            entity.Property(e => e.ActivityId).ValueGeneratedOnAdd();
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.IsCompleted).IsRequired();
            entity.Property(e => e.Detail).HasMaxLength(int.MaxValue);
            entity.Property(e => e.Priority).HasMaxLength(20);

            // Configure relationship with TodoItem
            entity.HasOne(e => e.TodoItem)
                .WithMany(t => t.Activities)
                .HasForeignKey(e => e.TodoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
