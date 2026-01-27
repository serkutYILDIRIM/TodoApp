namespace TodoApp.API.DTOs;

public class UpdateActivityDto
{
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public string? Detail { get; set; }
    public string? Priority { get; set; }
}
