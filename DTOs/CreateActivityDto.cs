namespace TodoApp.API.DTOs;

public class CreateActivityDto
{
    public int TodoId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Detail { get; set; }
    public string? Priority { get; set; }
}
