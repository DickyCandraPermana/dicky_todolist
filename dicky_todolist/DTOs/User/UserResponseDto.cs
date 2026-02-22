using System.ComponentModel.DataAnnotations;

namespace dicky_todolist.DTOs.User;

public record class UserResponseDTO
(
  [Required] Guid Id,
  string Username,
  [EmailAddress] string Email,
  DateTime CreatedAt,
  DateTime UpdatedAt,
  DateTime? DeletedAt
);
