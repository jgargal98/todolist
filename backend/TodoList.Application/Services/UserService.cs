using TodoList.Application.Services.Interfaces;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;

namespace TodoList.Application.Services;

/// <summary>
/// Orchestrates user operations between the API and the Repository.
/// </summary>
public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
    {
        // Business logic or DTO mapping would happen here
        return await userRepository.GetAllUsersAsync();
    }
}