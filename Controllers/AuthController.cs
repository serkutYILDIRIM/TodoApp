using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.API.Data;
using TodoApp.API.DTOs;

namespace TodoApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly TodoAppDbContext _context;

    public AuthController(TodoAppDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Unauthorized(new { message = "Username and password are required" });
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        return Ok(new
        {
            userId = user.UserId,
            username = user.Username,
            message = "Login successful"
        });
    }
}
