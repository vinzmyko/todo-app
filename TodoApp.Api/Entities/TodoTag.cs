namespace TodoApp.Api.Entities;

public class TodoTag
{
    public int TodoId { get; set; }
    public required Todo Todo { get; set; }

    public int TagId { get; set; }
    public required Tag Tag { get; set; }
}
