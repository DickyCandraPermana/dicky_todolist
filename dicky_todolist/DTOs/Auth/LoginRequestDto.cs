using System.ComponentModel.DataAnnotations;

namespace dicky_todolist.DTOs.Auth;

public record class LoginRequestDto
(
  [EmailAddress] string Email,
  string Password
);
