namespace dicky_todolist.DTOs;

public record ErrorResponseDto
{
  public int StatusCode { get; init; }
  public string Message { get; init; }

  public string? Details { get; init; }

  public DateTime Timestamp { get; init; } = DateTime.UtcNow;

  public ErrorResponseDto(int statusCode, string message, string? details = null)
  {
    StatusCode = statusCode;
    Message = message;
    Details = details;
  }
}