namespace dicky_todolist.DTOs;

public record ValidationErrorResponseDto : ErrorResponseDto
{
  public IDictionary<string, string[]> Errors { get; init; }

  public ValidationErrorResponseDto(int statusCode, string message, IDictionary<string, string[]> errors)
      : base(statusCode, message)
  {
    Errors = errors;
  }
}