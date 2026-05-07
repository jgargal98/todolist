using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoList.Infrastructure.Data;

namespace TodoList.Infrastructure;

/// <summary>
/// Provides helper methods to initialize the database schema at application startup.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Synchronizes the database schema using Entity Framework migrations.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve dependencies.</param>
    /// <param name="isDevelopment">Indicates if the application is running in a development environment.</param>
    public static void InitializeDatabase(this IServiceProvider serviceProvider, bool isDevelopment)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        var logger = services.GetRequiredService<ILogger<AppDbContext>>();

        try
        {
            // -------------------------------------------------------------------------
            // !!! WARNING: DEVELOPMENT ONLY LOGIC !!!
            // The following block deletes the entire database to ensure a clean state
            // if the schema has changed. REMOVE THIS BLOCK BEFORE FINAL RELEASE.
            // -------------------------------------------------------------------------
            logger.LogWarning("Dropping database to recreate schema from scratch...");
            context.Database.EnsureDeleted();
            // -------------------------------------------------------------------------
            // !!! END OF DELETE LOGIC !!!
            // -------------------------------------------------------------------------

            logger.LogInformation("Applying pending migrations to Azure SQL...");
            context.Database.Migrate();
            logger.LogInformation("Database is synchronized and ready.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw; // Fail-fast to prevent the app from running with an invalid schema
        }
    }
}