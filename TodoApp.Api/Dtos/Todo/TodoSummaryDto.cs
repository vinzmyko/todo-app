using TodoApp.Api.Entities;

namespace TodoApp.Api.Dtos.Todo;

public record TodoSummaryDto(
    int Id,
    string Title,
    bool IsCompleted,
    DateTime? DueDate,
    Timeframe TodoTimeFrame
);