using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Entities;

public class Todo
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public Timeframe TodoTimeFrame { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<TodoTag> TodoTags { get; set; } = new List<TodoTag>();
}

public enum Timeframe
{
    Today,
    NextSevenDays,
    Future
}