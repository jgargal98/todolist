using TodoList.Domain.Entities;

namespace TodoList.Application.Services.Interfaces;

/// <summary>
/// Service interface for user-related business logic.
/// </summary>
public interface IUserService
{
    Task<IEnumerable<ApplicationUser>> GetUsersAsync();
}