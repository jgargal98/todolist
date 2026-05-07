using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoList.Domain.Entities;
using TodoList.Infrastructure.Data;

namespace TodoList.Infrastructure;

/// <summary>
/// Provides utility methods to handle database migrations and initial data seeding.
/// This class ensures the database schema is up-to-date and contains essential records.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Synchronizes the database schema and performs data seeding.
    /// </summary>
    /// <param name="serviceProvider">The root service provider to resolve dependencies.</param>
    /// <param name="isDevelopment">A flag indicating if the environment is Development.</param>
    /// <exception cref="Exception">Throws an exception if the migration or seeding process fails.</exception>
    public static void InitializeDatabase(this IServiceProvider serviceProvider, bool isDevelopment)
    {
        // Creating a new scope to resolve scoped services safely
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        // Resolving required infrastructure services
        var context = services.GetRequiredService<AppDbContext>();
        var logger = services.GetRequiredService<ILogger<AppDbContext>>();
        var configuration = services.GetRequiredService<IConfiguration>();

        try
        {
            // WARNING: In Development, we reset the database to ensure schema consistency
            if (isDevelopment)
            {
                logger.LogWarning("Development mode detected: Dropping and recreating database...");
                context.Database.EnsureDeleted();
            }

            // Applying any pending EF Core migrations to the SQL Server / Azure SQL instance
            logger.LogInformation("Applying pending migrations to the database...");
            context.Database.Migrate();

            // Executing the seed logic for the administrative user
            // We use .GetAwaiter().GetResult() to block the startup until seeding completes
            SeedAdminUserAsync(services, logger, configuration).GetAwaiter().GetResult();

            logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "A fatal error occurred during the database initialization process.");
            throw; // Fail-fast to prevent the application from running in an inconsistent state
        }
    }

    /// <summary>
    /// Seeds a default Administrator user using credentials from environment variables or configuration.
    /// </summary>
    /// <param name="services">Service provider to resolve the UserManager.</param>
    /// <param name="logger">Logger for reporting seeding status.</param>
    /// <param name="configuration">Configuration to access environment-specific secrets.</param>
    private static async Task SeedAdminUserAsync(IServiceProvider services, ILogger logger, IConfiguration configuration)
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // Retrieving credentials from IConfiguration (Environment Variables or AppSettings)
        // Azure Key: SeedData__AdminEmail / SeedData__AdminPassword
        string adminEmail = configuration["SeedData:AdminEmail"] ?? "admin@todolist.com";
        string adminPassword = configuration["SeedData:AdminPassword"] ?? "123";

        // Requirement: LINQ Method Syntax to check for existing users
        if (!await userManager.Users.AnyAsync(u => u.Email == adminEmail))
        {
            logger.LogInformation("No administrative user found. Seeding initial admin...");

            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true // Bypassing email verification for the seed user
            };

            // Requirement: Identity implementation for secure password hashing
            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                logger.LogInformation("Administrative user {Email} created successfully.", adminEmail);
            }
            else
            {
                // Detailed error logging in case Identity password requirements are not met
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                logger.LogError("Failed to seed admin user. Errors: {Errors}", errors);
            }
        }
    }
}