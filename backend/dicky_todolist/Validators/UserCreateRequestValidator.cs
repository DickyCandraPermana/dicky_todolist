using dicky_todolist.DTOs.User;
using FluentValidation;

namespace dicky_todolist.Validators;

public class UserCreateRequestValidator : AbstractValidator<UserCreateRequestDto>
{
  public UserCreateRequestValidator()
  {
    RuleFor(x => x.Username)
        .NotEmpty().WithMessage("Username tidak boleh kosong")
        .MinimumLength(3).WithMessage("Username minimal 3 karakter");

    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email wajib diisi")
        .EmailAddress().WithMessage("Format email tidak valid");

    RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password tidak boleh kosong")
        .MinimumLength(6).WithMessage("Password minimal 6 karakter");
  }
}