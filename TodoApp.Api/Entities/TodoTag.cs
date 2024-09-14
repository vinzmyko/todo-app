namespace TodoApp.Api.Entities;

public class TodoTag
{
    public int TodoId { get; set; }
    public Todo Todo { get; set; } = null!;

    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}