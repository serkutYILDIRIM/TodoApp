namespace TodoApp.API.DTOs;

public class CreateTodoItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Detail { get; set; }
    public string? Priority { get; set; }
}
