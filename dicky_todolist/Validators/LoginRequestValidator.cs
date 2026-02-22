using System;
using dicky_todolist.DTOs.Auth;
using FluentValidation;

namespace dicky_todolist.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
  public LoginRequestValidator()
  {
    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email tidak boleh kosong!")
        .EmailAddress().WithMessage("Format email tidak valid");

    RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password tidak boleh kosong")
        .MinimumLength(6).WithMessage("Password minimal 6 karakter");
  }
}
