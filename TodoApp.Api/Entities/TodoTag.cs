using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Api.Entities;

public class TodoTag
{
    // Composite Key
    public int TodoId { get; set; }
    public int TagId { get; set; }
    [ForeignKey("TodoId")] public required Todo Todo { get; set; }
    [ForeignKey("TagId")] public required Tag Tag { get; set; }
}
