using System.ComponentModel.DataAnnotations;

namespace dicky_todolist.DTOs.Todo;

public record class TodoCreateRequestDto
(
  [Required] string Title,
  string? Description
);