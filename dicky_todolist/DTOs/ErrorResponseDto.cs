namespace dicky_todolist.DTOs;

public record ErrorResponseDto
{
  public int StatusCode { get; init; }
  public string Message { get; init; }

  // Details bisa berisi StackTrace (hanya di Development)
  // atau pesan error spesifik lainnya.
  public string? Details { get; init; }

  // Timestamp membantu dalam pelacakan log
  public DateTime Timestamp { get; init; } = DateTime.UtcNow;

  public ErrorResponseDto(int statusCode, string message, string? details = null)
  {
    StatusCode = statusCode;
    Message = message;
    Details = details;
  }
}