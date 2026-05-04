using Microsoft.EntityFrameworkCore;
using MyNotesApp.Data;
using MyNotesApp.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure DB connection (Lee de variables de entorno para Docker)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
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
app.MapGet("/", () => "hello world!");

app.MapGet("/test-db", async (AppDbContext db) =>{
    try{
        var count = await db.Users.CountAsync();
        return Results.Ok(new { mensaje = "Conecction success", totalUsuarios = count });
    }
    catch (Exception ex){
        return Results.Problem($"Connection error: {ex.Message}");
    }
});

app.Run();