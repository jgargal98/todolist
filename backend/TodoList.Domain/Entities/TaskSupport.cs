namespace TodoList.Domain.Entities;

/// <summary>
/// Represents the possible states of a task.
/// </summary>
public enum TaskStatus
{
    /// <summary>Task is newly created and not yet started.</summary>
    Pending = 1,
    /// <summary>Task is currently being worked on.</summary>
    InProgress = 2,
    /// <summary>Task is waiting for an external factor.</summary>
    OnHold = 3,
    /// <summary>Task has been finished.</summary>
    Completed = 4,
    /// <summary>Task is no longer required.</summary>
    Canceled = 5
}

/// <summary>
/// Represents a simple subtask structure to be stored within a TaskItem as JSON.
/// </summary>
public class SubTask
{
    /// <summary>
    /// Title of the subtask.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the subtask is finished.
    /// </summary>
    public bool IsDone { get; set; } = false;
}