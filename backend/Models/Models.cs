using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace TodoList.Models;

// Enumeration for Task
public enum TaskStatus
{
    NotStarted = 0,
    InProgress = 1,
    Paused = 2,
    Late = 3,
    Finished = 4
}

// Represents a sub-item within a Task. 
// This will be stored as a JSON string in the database.
public class SubTaskInfo
{
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}

// Tags used to categorize tasks
public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Foreign Key: Links to AspNetUsers table (Identity uses strings for IDs)
    public string UserId { get; set; } = string.Empty;

    // Navigation property for the User
    public IdentityUser? User { get; set; }

    // Many-to-Many relationship: A Tag can be linked to many TodoTasks
    // [JsonIgnore] prevents circular reference errors during API serialization
    [JsonIgnore]
    public List<TodoTask> Tasks { get; set; } = new();
}

// The main Task entity
public class TodoTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

    // This list will be mapped as a JSON column in SQL Server
    public List<SubTaskInfo> SubTasks { get; set; } = new();

    // Foreign Key: Identifies the owner of the task
    public string UserId { get; set; } = string.Empty;

    // Navigation property for the owner
    public IdentityUser? User { get; set; }

    // Many-to-Many relationship: A Task can have multiple Tags
    public List<Tag> Tags { get; set; } = new();
}