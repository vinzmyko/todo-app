namespace TodoApp.Api.Entities;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}