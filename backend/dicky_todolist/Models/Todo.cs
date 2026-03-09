using System.ComponentModel.DataAnnotations;

namespace dicky_todolist.Models;

public class Todo
{
  [Required]
  public Guid Id { get; set; } = Guid.NewGuid();
  public required string Title { get; set; }
  public string Description { get; set; } = string.Empty;
  public bool IsCompleted { get; set; } = false;
  public Guid UserId { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
  public DateTime? DeletedAt { get; set; }
  public User User { get; set; } = null!;
}