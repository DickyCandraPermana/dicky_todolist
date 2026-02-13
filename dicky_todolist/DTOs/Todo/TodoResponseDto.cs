using System.ComponentModel.DataAnnotations;

namespace dicky_todolist.DTOs.Todo;

public record class TodoResponseDto
(
  [Required] Guid Id,
  [Required] string Title,
  string Description,
  [Required] bool IsCompleted,
  [Required] DateTime CreatedAt
);
