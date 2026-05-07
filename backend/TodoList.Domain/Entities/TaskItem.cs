using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.Domain.Entities;

/// <summary>
/// Represents a task item created by a user, including its subtasks and status.
/// </summary>
public class TaskItem
{
    /// <summary>
    /// Unique identifier for the task.
    /// </summary>
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Title or summary of the task.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the task.
    /// </summary>
    public string? Desc { get; set; }

    /// <summary>
    /// Date and time when the task is due.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Current status of the task using the TaskStatus enum.
    /// </summary>
    [Required]
    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    /// <summary>
    /// Collection of subtasks stored as a JSON object in the database column.
    /// </summary>
    public virtual ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();

    /// <summary>
    /// Foreign key referencing the owner of the task (Identity User).
    /// </summary>
    [Required]
    public string IdUser { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property for the user who owns this task.
    /// </summary>
    [ForeignKey("IdUser")]
    public virtual ApplicationUser User { get; set; } = null!;

    /// <summary>
    /// Navigation property for the many-to-many relationship with Tags.
    /// </summary>
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}