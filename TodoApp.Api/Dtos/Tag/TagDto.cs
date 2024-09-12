using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Dtos.Tag;

public record class TagDto(
    int Id,
    [Required][StringLength(50, MinimumLength = 1)]string Name
);
