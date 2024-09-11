namespace TodoApp.Api.Entities;

public class Todo
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public required Timeframe TodoTimeFrame { get; set; }

    // Foreign Key from the table User
    public int UserId { get; set; }
}

public enum Timeframe
{
    Today,
    NextSevenDays,
    Future
}