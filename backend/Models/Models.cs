namespace TodoList.Models;

public enum TaskStatus { NotStarted = 0, InProgress = 1, Paused = 2, Late = 3, Finished = 4 }

public class SubTaskInfo
{
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PwHash { get; set; } = string.Empty;
}

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }

    // Relación N:N con Tareas
    public List<TodoTask> Tasks { get; set; } = new();
}

public class TodoTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

    // Esto SÍ es JSON (como en tu diagrama, es un campo dentro de Task)
    public List<SubTaskInfo> SubTasks { get; set; } = new();

    public int UserId { get; set; }

    // Relación N:N con Tags
    public List<Tag> Tags { get; set; } = new();
}