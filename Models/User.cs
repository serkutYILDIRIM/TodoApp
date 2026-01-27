namespace TodoApp.API.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }

    public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
}
