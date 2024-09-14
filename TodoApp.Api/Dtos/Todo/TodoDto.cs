using System.ComponentModel.DataAnnotations;
using TodoApp.Api.Dtos.Tag;
using TodoApp.Api.Entities;

namespace TodoApp.Api.Dtos.Todo;

public record class TodoDto(
    int Id,
    [Required][StringLength(100, MinimumLength = 1)]string Title,
    [Required][StringLength(1000)]string Description,
    bool IsCompleted,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    DateTime? DueDate,
    [Required]Timeframe TodoTimeFrame,
    int UserId,
    string UserName,
    IEnumerable<TagDto> Tags
);