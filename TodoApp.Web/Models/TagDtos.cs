using System.ComponentModel.DataAnnotations;

namespace TodoApp.Web.Models;

public class TagDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class CreateTagDto
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public required string Name { get; set; }
}