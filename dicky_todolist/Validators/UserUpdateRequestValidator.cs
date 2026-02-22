using System;
using dicky_todolist.DTOs.User;
using FluentValidation;

namespace dicky_todolist.Validators;

public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequestDto>
{
  public UserUpdateRequestValidator()
  {
    RuleFor(x => x.Username)
        .MinimumLength(3).WithMessage("Username minimal 3 karakter");

    RuleFor(x => x.Email)
        .EmailAddress().WithMessage("Format email tidak valid");

    RuleFor(x => x.Password)
        .MinimumLength(6).WithMessage("Password minimal 6 karakter");
  }
}
