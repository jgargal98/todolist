namespace TodoList.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Services.Interfaces;

/// <summary>
/// Controller for managing user-related test endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")] // /api/users
public class UsersController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Returns a list of all registered users.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetUsersAsync();
        return Ok(users);
    }
}