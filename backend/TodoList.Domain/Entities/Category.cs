using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.Domain.Entities;

/// <summary>
/// Represents a category used to organize tasks or user information.
/// </summary>
public class Category
{
    /// <summary>
    /// Unique identifier for the category.
    /// </summary>
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Name of the category.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key referencing the owner of the category.
    /// </summary>
    [Required]
    public string IdUser { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property for the user who owns this category.
    /// </summary>
    [ForeignKey("IdUser")]
    public virtual ApplicationUser User { get; set; } = null!;
}