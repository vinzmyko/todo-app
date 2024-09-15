using System.ComponentModel.DataAnnotations;

namespace TodoApp.Web.Models;

public class UserDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateUserDto
{
    [Required]
    [StringLength(50)]
    public required string Username { get; set; }
}
