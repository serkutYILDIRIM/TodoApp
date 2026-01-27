namespace TodoApp.API.DTOs;

public class TodoItemDto
{
    public int TodoId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsCompleted { get; set; }
    public string? Detail { get; set; }
    public string? Priority { get; set; }
    public int ActivityCount { get; set; }
    public int CompletedActivityCount { get; set; }
}
