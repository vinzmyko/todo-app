using TodoApp.Api.Entities;
using TodoApp.Api.Dtos.User;

namespace TodoApp.Api.Mapping;

public static class UserMapping
{
    public static User ToEntity(this CreateUserDto userDto)
    {
        return new User
        {
            Username = userDto.Username,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static UserDto ToUserDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.Username,
            user.CreatedAt,
            user.LastLoginAt
        );
    }

    // If you need to update an existing User entity
    public static void UpdateEntityFromDto(this User user, CreateUserDto updateDto)
    {
        user.Username = updateDto.Username;
        // Note: Be cautious about updating CreatedAt or LastLoginAt here
    }
}