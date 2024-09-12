using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Entities;

public class Tag
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
}
