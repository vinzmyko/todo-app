using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Dtos.User;

public record class UserDto(
    int Id,
    [Required][StringLength(50, MinimumLength = 3)]string Username,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);
