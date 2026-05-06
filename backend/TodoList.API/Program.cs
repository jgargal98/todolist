// 1. IMPORTANTE: Ahora los usings deben apuntar a tus nuevas capas
using Microsoft.EntityFrameworkCore;
// using TodoList.Infrastructure.Data; // Descomenta esto cuando crees tu AppDbContext ahí
// using TodoList.Domain.Entities;     // Descomenta esto cuando crees tu User ahí

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE SERVICIOS (El "Contenedor") ---

// ¡NUEVO! Le decimos a .NET que busque y prepare nuestros [ApiController]
builder.Services.AddControllers();

// Mantenemos OpenAPI (para que tengas documentación de tu API automática)
builder.Services.AddOpenApi();

/*
// Tu configuración de Base de Datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("¡La cadena de conexión está vacía! Revisa la configuración en Azure/appsettings.json.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure()));

*/


// Tu configuración de CORS (Permitir que Angular se conecte)
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

// --- CONFIGURACIÓN DEL PIPELINE HTTP (El "Túnel" de peticiones) ---
app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors();

//Mapear los Controladores (conecta las rutas como /api/Auth con tus clases)
app.MapControllers();

/*using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}*/


// --- TU CÓDIGO DE INICIALIZACIÓN Y PRUEBAS ---
// "Hello World"
// Endpoints de prueba (Minimal APIs)
app.MapGet("/api/", () => "Si lees esto me debes 20 pavos");

/*
app.MapGet("/test-db", async (AppDbContext db) =>
{
    try
    {
        var count = await db.Users.CountAsync();
        return Results.Ok(new { mensaje = "Connection success", totalUsuarios = count });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Connection error: {ex.Message}");
    }
});


// BORRAR AL EMPEZAR A PROBAR LA API EN SERIO
app.MapGet("/reset-db", (AppDbContext db) =>
{
    try
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        return Results.Ok(new { mensaje = "Base de datos borrada y reconstruida" });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al reiniciar: {ex.Message}");
    }
});*/

app.Run();