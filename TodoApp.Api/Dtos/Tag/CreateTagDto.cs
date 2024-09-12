using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Dtos.Tag;

public record class CreateTagDto(
    [Required][StringLength(50, MinimumLength = 1)]string Name
);