using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Entities;

namespace TodoApp.Api.Data;

public class TodoAppDbContext : DbContext
{
    public TodoAppDbContext(DbContextOptions<TodoAppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TodoTag> TodoTags => Set<TodoTag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the TodoTag entity (junction table for many-to-many relationship between Todo and Tag)
        modelBuilder.Entity<TodoTag>(entity =>
        {
            // Set up composite key consisting of TodoId and TagId
            entity.HasKey(tt => new { tt.TodoId, tt.TagId });

            // Configure the relationship between TodoTag and Todo
            entity.HasOne(tt => tt.Todo)         // TodoTag has one Todo
                .WithMany(t => t.TodoTags)       // Todo has many TodoTags
                .HasForeignKey(tt => tt.TodoId)  // Foreign key in TodoTag
                .OnDelete(DeleteBehavior.Cascade); // If Todo is deleted, delete related TodoTags

            // Configure the relationship between TodoTag and Tag
            entity.HasOne(tt => tt.Tag)          // TodoTag has one Tag
                .WithMany(t => t.TodoTags)       // Tag has many TodoTags
                .HasForeignKey(tt => tt.TagId)   // Foreign key in TodoTag
                .OnDelete(DeleteBehavior.Cascade); // If Tag is deleted, delete related TodoTags
        });

        // Configure the relationship between Todo and User
        modelBuilder.Entity<Todo>()
            .HasOne(t => t.User)       // Todo has one User
            .WithMany()                // User can have many Todos (but we don't navigate back to Todos from User)
            .HasForeignKey(t => t.UserId); // Foreign key in Todo

        modelBuilder.Entity<Tag>().HasData(
            new { Id = 1, Name = "work"},
            new { Id = 2, Name = "personal"},
            new { Id = 3, Name = "urgent"},
            new { Id = 4, Name = "shopping"},
            new { Id = 5, Name = "fitness"}
        );
    }
}