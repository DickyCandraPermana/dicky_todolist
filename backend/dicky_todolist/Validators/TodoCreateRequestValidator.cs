using dicky_todolist.DTOs.Todo;
using FluentValidation;

namespace dicky_todolist.Validators;

public class TodoCreateRequestValidator : AbstractValidator<TodoCreateRequestDto>
{
  public TodoCreateRequestValidator()
  {
    RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Judul tidak boleh kosong");
  }
}
