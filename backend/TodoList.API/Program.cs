using TodoList.Infrastructure;
using TodoList.Application.Services;
using System.Reflection;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVICE CONFIGURATION (Dependency Injection Container) ---

// --- REGISTER LAYERS ---
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

/// <summary>
/// Configure CORS using an environment variable for better security.
/// </summary>
var frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:3000"; // Default for local dev

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

/// <summary>
/// Configure Swagger/OpenAPI for interactive documentation.
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ToDo List API",
        Version = "v1",
        Description = "Clean Architecture API with Identity and JSON Subtasks support."
    });

    // Enable XML comments for Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// --- 2. MIDDLEWARE PIPELINE (Request Handling) ---
/// <summary>
/// Define the request processing pipeline using Middlewares.
/// </summary>
// Enables Swagger UI in both Development and Production (helpful for tutors)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
// Use CORS before Authentication
app.UseCors("AllowFrontend");
// Crucial: Identification of the user
app.UseAuthentication();
// Crucial: Checking user permissions
app.UseAuthorization();
// Route requests to Controller actions
app.MapControllers();

// --- 3. DATABASE CREATION AND MANAGEMENT ---
/// <summary>
/// Automatically applies migrations at startup to ensure the database is ready.
/// </summary>
app.Services.InitializeDatabase(app.Environment.IsDevelopment());

app.Run();