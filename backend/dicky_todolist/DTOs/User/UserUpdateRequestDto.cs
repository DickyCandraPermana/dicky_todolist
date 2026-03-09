using System.ComponentModel.DataAnnotations;

namespace dicky_todolist.DTOs.User;

public record class UserUpdateRequestDto
(
  string? Username,
  [EmailAddress] string? Email,
  string? Password
);
