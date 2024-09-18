using System.ComponentModel.DataAnnotations;
using TodoApp.Api.Entities;

namespace TodoApp.Api.Dtos.Todo;

public record UpdateTodoDto(
    [Required][StringLength(100, MinimumLength = 1)] string Title,
    [Required][StringLength(1000)] string Description,
    DateTime? DueDate,
    [Required] Timeframe TodoTimeFrame,
    IEnumerable<int> TagIds,
    string UserTimeZone,
    int UserOffset
);
