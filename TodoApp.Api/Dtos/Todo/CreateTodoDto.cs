using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Dtos.Todo;

public record class CreateTodoDto(
    [Required][StringLength(100, MinimumLength = 1)]string Title,
    [Required][StringLength(1000)]string Description,
    DateTime? DueDate,
    [Required]int UserId,
    IEnumerable<int> TagIds,
    string UserTimeZone,
    int UserTimeOffsetMinutes,
    DateTime UserLocalTime
);