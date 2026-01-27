using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.API.Models;

[Table("TodoItems")]
public class TodoItem
{
    public int TodoId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsCompleted { get; set; }
    public string? Detail { get; set; }
    public string? Priority { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
