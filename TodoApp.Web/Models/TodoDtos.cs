using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoApp.Web.Models;

public class TodoDto
{
    public int Id { get; set; }
    [Required] [StringLength(100, MinimumLength = 5)] public required string Title { get; set; }
    [StringLength(1000)] public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    [Required] public required DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? DueDate { get; set; }
    [Required] public required TimeFrame TodoTimeFrame { get; set; }
    [Required] public required int UserId { get; set; }
    [JsonIgnore]
     public UserDto? User { get; set; }
    public List<TagDto>? Tags { get; set; }
}

public class CreateTodoDto
{
    [Required] [StringLength(100, MinimumLength = 1)] public required string Title { get; set; }

    [Required] [StringLength(1000)] public required string Description { get; set; }

    public DateTime? DueDate { get; set; }

    [Required] public required int UserId { get; set; }

    public IEnumerable<int>? TagIds { get; set; }

    public required string UserTimeZone { get; set; }

    public required int UserTimeOffsetMinutes { get; set; }

    public required DateTime UserLocalTime { get; set; }
}


public class UpdateTodoDto
{
    public int Id { get; set; }
    [Required] [StringLength(100, MinimumLength = 5)] public required string Title { get; set; }
    [StringLength(1000)] public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? DueDate { get; set; }
    [Required] public required TimeFrame TodoTimeFrame { get; set; }
    public List<int>? TagIds { get; set; }
}

public class TodoSummaryDto
{
    public int Id { get; set; }
    [Required] [StringLength(100, MinimumLength = 5)] public required string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    [Required] public required TimeFrame TodoTimeFrame { get; set; }
    public List<TagDto>? Tags { get; set; }
}

public enum TimeFrame
{
    Today,
    NextSevenDays,
    Future
}