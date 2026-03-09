namespace dicky_todolist.DTOs.Todo;

public record class TodoUpdateRequestDto(
  string? Title,
  string? Description,
  bool? IsCompleted
);
