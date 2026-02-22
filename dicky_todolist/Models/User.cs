using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace dicky_todolist.Models;

public class User
{
  [Required]
  public Guid Id { get; set; } = Guid.NewGuid();
  public required string Username { get; set; }
  public required string Email { get; set; }
  public required string Password { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
  public DateTime? DeletedAt { get; set; }

  public ICollection<Todo> Todos { get; set; } = new List<Todo>();
}
