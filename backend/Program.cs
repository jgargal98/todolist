using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;

var builder = WebApplication.CreateBuilder(args);

// .NET buscará en las Connection Strings de Azure primero.
// Si no la encuentra ahí, buscará en el appsettings.json.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("¡La cadena de conexión está vacía! Revisa la configuración en Azure.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Configure CORS (Permitir que Angular se conecte)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 3. Habilitar CORS
app.UseCors();

// 4. "Hello World" de Base de Datos
// Al arrancar, intenta crear la DB y las tablas si no existen
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Esto aplica el script SQL que vimos antes de forma automática
    db.Database.EnsureCreated();
}

// 5. Endpoint de prueba (Health Check)

app.MapGet("/", () => "Si lees esto me debes 20 pavos");

app.MapGet("/test-db", async (AppDbContext db) =>
{
    try
    {
        var count = await db.Users.CountAsync();
        return Results.Ok(new { mensaje = "Conecction success", totalUsuarios = count });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Connection error: {ex.Message}");
    }
});





app.Run();