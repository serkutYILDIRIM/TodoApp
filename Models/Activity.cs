namespace TodoApp.API.Models;

public class Activity
{
    public int ActivityId { get; set; }
    public int TodoId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsCompleted { get; set; }
    public string? Detail { get; set; }
    public string? Priority { get; set; }

    public TodoItem TodoItem { get; set; } = null!;
}
