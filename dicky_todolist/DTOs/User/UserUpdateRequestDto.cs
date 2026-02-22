using System.ComponentModel.DataAnnotations;

namespace dicky_todolist.DTOs.User;

public record class UserUpdateRequestDto
(
  [Required] Guid Id,
  string Username,
  [EmailAddress] string Email,
  string Password
);
