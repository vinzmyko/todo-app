using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Entities;

public class Tag
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public ICollection<TodoTag> TodoTags { get; set; } = new List<TodoTag>();
}