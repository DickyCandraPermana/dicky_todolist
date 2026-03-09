using dicky_todolist.DTOs.Todo;
using FluentValidation;

namespace dicky_todolist.Validators;

public class TodoUpdateRequestValidator : AbstractValidator<TodoUpdateRequestDto>
{
  public TodoUpdateRequestValidator()
  {
    RuleFor(x => x.Title)
        .NotEmpty().WithMessage("Judul catatan tidak boleh kosong!");
  }
}
