using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Entities;

namespace TodoApp.Api.Data;

public class TodoAppDbContext(DbContextOptions<TodoAppDbContext> options) : DbContext(options)
{
    // Create the tables
    // When I reference DbSet<User> Users I am able to manipulate the table
    public DbSet<User> Users => Set<User>();
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TodoTag> TodoTags => Set<TodoTag>();

    // When you execute the migration this is the code that will execute
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define Composite Primary Key for TodoTags
        modelBuilder.Entity<TodoTag>()
            .HasKey(tt => new { tt.TodoId, tt.TagId });

        // Configure many-to-many relationship
        modelBuilder.Entity<TodoTag>()
            .HasOne(tt => tt.Todo) // Each TodoTag(tt) has one Todo
            .WithMany() // Todo can have many TodoTag entries
            .HasForeignKey(tt => tt.TodoId); // Defines TodoId as the FK for this relationship

        modelBuilder.Entity<TodoTag>()
            .HasOne(tt => tt.Tag)
            .WithMany()
            .HasForeignKey(tt => tt.TagId);

        // Init Tags table with these values
        modelBuilder.Entity<Tag>().HasData(
            new { Id = 1, Name = "work"},
            new { Id = 2, Name = "personal"},
            new { Id = 3, Name = "urgent"},
            new { Id = 4, Name = "shopping"},
            new { Id = 5, Name = "fitness"}
        );
    }
}