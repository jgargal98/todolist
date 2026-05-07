namespace TodoList.Domain.Interfaces;

using TodoList.Domain.Entities;

/// <summary>
/// Defines the contract for user data persistence operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves all users using LINQ Method Syntax.
    /// </summary>
    /// <returns>A collection of ApplicationUser entities.</returns>
    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
}