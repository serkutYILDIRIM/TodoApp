using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.API.Data;
using TodoApp.API.DTOs;
using TodoApp.API.Models;

namespace TodoApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivitiesController : ControllerBase
{
    private readonly TodoAppDbContext _context;

    public ActivitiesController(TodoAppDbContext context)
    {
        _context = context;
    }

    // GET: api/activities/todo/{todoId}
    [HttpGet("todo/{todoId}")]
    public async Task<ActionResult<IEnumerable<ActivityDto>>> GetActivitiesByTodoItem(int todoId)
    {
        var activities = await _context.Activities
            .Where(a => a.TodoId == todoId)
            .OrderByDescending(a => a.CreatedDate)
            .Select(a => new ActivityDto
            {
                ActivityId = a.ActivityId,
                TodoId = a.TodoId,
                Title = a.Title,
                CreatedDate = a.CreatedDate,
                IsCompleted = a.IsCompleted,
                Detail = a.Detail,
                Priority = a.Priority
            })
            .ToListAsync();

        return Ok(activities);
    }

    // GET: api/activities/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityDto>> GetActivity(int id)
    {
        var activity = await _context.Activities
            .Where(a => a.ActivityId == id)
            .Select(a => new ActivityDto
            {
                ActivityId = a.ActivityId,
                TodoId = a.TodoId,
                Title = a.Title,
                CreatedDate = a.CreatedDate,
                IsCompleted = a.IsCompleted,
                Detail = a.Detail,
                Priority = a.Priority
            })
            .FirstOrDefaultAsync();

        if (activity == null)
            return NotFound(new { message = "Activity not found" });

        return Ok(activity);
    }

    // POST: api/activities
    [HttpPost]
    public async Task<ActionResult<ActivityDto>> CreateActivity([FromBody] CreateActivityDto createDto)
    {
        var activity = new Activity
        {
            TodoId = createDto.TodoId,
            Title = createDto.Title,
            Detail = createDto.Detail,
            Priority = createDto.Priority,
            CreatedDate = DateTime.Now,
            IsCompleted = false
        };

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        var activityDto = new ActivityDto
        {
            ActivityId = activity.ActivityId,
            TodoId = activity.TodoId,
            Title = activity.Title,
            CreatedDate = activity.CreatedDate,
            IsCompleted = activity.IsCompleted,
            Detail = activity.Detail,
            Priority = activity.Priority
        };

                return CreatedAtAction(nameof(GetActivity), new { id = activity.ActivityId }, activityDto);
            }

            // PUT: api/activities/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateActivity(int id, [FromBody] UpdateActivityDto updateDto)
            {
                var activity = await _context.Activities.FindAsync(id);

                if (activity == null)
                {
                    return NotFound(new { message = "Activity not found" });
                }

                activity.Title = updateDto.Title;
                activity.IsCompleted = updateDto.IsCompleted;
                activity.Detail = updateDto.Detail;
                activity.Priority = updateDto.Priority;

                _context.Activities.Update(activity);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            // DELETE: api/activities/{id}
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteActivity(int id)
            {
                var activity = await _context.Activities.FindAsync(id);

                if (activity == null)
                {
                    return NotFound(new { message = "Activity not found" });
                }

                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            // PATCH: api/activities/{id}/toggle
            [HttpPatch("{id}/toggle")]
            public async Task<IActionResult> ToggleActivityCompletion(int id)
            {
                var activity = await _context.Activities.FindAsync(id);

                if (activity == null)
                {
                    return NotFound(new { message = "Activity not found" });
                }

                activity.IsCompleted = !activity.IsCompleted;

                _context.Activities.Update(activity);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
