using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Dtos.User;

public record class CreateUserDto(
    [Required][StringLength(50, MinimumLength = 3)]string Username
);
