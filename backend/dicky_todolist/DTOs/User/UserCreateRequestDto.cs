using System.ComponentModel.DataAnnotations;

namespace dicky_todolist.DTOs.User;

public record class UserCreateRequestDto
(
  [Required] string Username,
  [Required][EmailAddress] string Email,
  [Required] string Password
);
