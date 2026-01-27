using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.API.Data;
using TodoApp.API.DTOs;
using TodoApp.API.Models;

namespace TodoApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoAppDbContext _context;

    public TodoItemsController(TodoAppDbContext context)
    {
        _context = context;
    }

    // GET: api/todoitems/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItemsByUser(int userId)
    {
        var todoItems = await _context.TodoItems
            .Include(t => t.Activities)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedDate)
            .Select(t => new TodoItemDto
            {
                TodoId = t.TodoId,
                UserId = t.UserId,
                Title = t.Title,
                CreatedDate = t.CreatedDate,
                IsCompleted = t.IsCompleted,
                Detail = t.Detail,
                Priority = t.Priority,
                ActivityCount = t.Activities.Count,
                CompletedActivityCount = t.Activities.Count(a => a.IsCompleted)
            })
            .ToListAsync();

        return Ok(todoItems);
    }

    // GET: api/todoitems/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetTodoItem(int id)
    {
        var todoItem = await _context.TodoItems
            .Include(t => t.Activities)
            .Where(t => t.TodoId == id)
            .Select(t => new TodoItemDto
            {
                TodoId = t.TodoId,
                UserId = t.UserId,
                Title = t.Title,
                CreatedDate = t.CreatedDate,
                IsCompleted = t.IsCompleted,
                Detail = t.Detail,
                Priority = t.Priority,
                ActivityCount = t.Activities.Count,
                CompletedActivityCount = t.Activities.Count(a => a.IsCompleted)
            })
            .FirstOrDefaultAsync();

        if (todoItem == null)
        {
            return NotFound(new { message = "TodoItem not found" });
        }

        return Ok(todoItem);
    }

    // POST: api/todoitems?userId={userId}
    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> CreateTodoItem([FromQuery] int userId, [FromBody] CreateTodoItemDto createDto)
    {
        var todoItem = new TodoItem
        {
            UserId = userId,
            Title = createDto.Title,
            Detail = createDto.Detail,
            Priority = createDto.Priority,
            CreatedDate = DateTime.Now,
            IsCompleted = false
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        var todoItemDto = new TodoItemDto
        {
            TodoId = todoItem.TodoId,
            UserId = todoItem.UserId,
            Title = todoItem.Title,
            CreatedDate = todoItem.CreatedDate,
            IsCompleted = todoItem.IsCompleted,
            Detail = todoItem.Detail,
            Priority = todoItem.Priority,
            ActivityCount = 0,
            CompletedActivityCount = 0
        };

                return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.TodoId }, todoItemDto);
            }

            // PUT: api/todoitems/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateTodoItem(int id, [FromBody] UpdateTodoItemDto updateDto)
            {
                var todoItem = await _context.TodoItems.FindAsync(id);

                if (todoItem == null)
                {
                    return NotFound(new { message = "TodoItem not found" });
                }

                todoItem.Title = updateDto.Title;
                todoItem.IsCompleted = updateDto.IsCompleted;
                todoItem.Detail = updateDto.Detail;
                todoItem.Priority = updateDto.Priority;

                _context.TodoItems.Update(todoItem);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            // DELETE: api/todoitems/{id}
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteTodoItem(int id)
            {
                var todoItem = await _context.TodoItems.FindAsync(id);

                if (todoItem == null)
                {
                    return NotFound(new { message = "TodoItem not found" });
                }

                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            // PATCH: api/todoitems/{id}/toggle
            [HttpPatch("{id}/toggle")]
            public async Task<IActionResult> ToggleTodoItemCompletion(int id)
            {
                var todoItem = await _context.TodoItems.FindAsync(id);

                if (todoItem == null)
                {
                    return NotFound(new { message = "TodoItem not found" });
                }

                todoItem.IsCompleted = !todoItem.IsCompleted;

                _context.TodoItems.Update(todoItem);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
