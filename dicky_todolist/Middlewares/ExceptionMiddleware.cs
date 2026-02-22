using System;
using FluentValidation;
using System.Net;
using System.Text.Json;
using dicky_todolist.DTOs;

namespace dicky_todolist.Middlewares;

public class ExceptionMiddleware
{
  private async Task HandleExceptionAsync(HttpContext context, Exception exception, IHostEnvironment env)
  {
    context.Response.ContentType = "application/json";

    // Default status code
    int statusCode = (int)HttpStatusCode.InternalServerError;
    string message = "Terjadi kesalahan internal pada server.";
    object response;

    if (exception is ValidationException valEx)
    {
      statusCode = (int)HttpStatusCode.BadRequest;

      // Ambil semua error dan kelompokkan berdasarkan nama field
      var errors = valEx.Errors
          .GroupBy(e => e.PropertyName)
          .ToDictionary(
              g => g.Key,
              g => g.Select(x => x.ErrorMessage).ToArray()
          );

      response = new ValidationErrorResponseDto(statusCode, "Validasi gagal", errors);
    }
    else
    {
      // Mapping Exception tertentu ke Status Code tertentu (Best Practice)
      if (exception is UnauthorizedAccessException) statusCode = (int)HttpStatusCode.Unauthorized;
      if (exception is KeyNotFoundException) statusCode = (int)HttpStatusCode.NotFound;

      // Buat DTO
      response = env.IsDevelopment()
          ? new ErrorResponseDto(statusCode, exception.Message, exception.StackTrace)
          : new ErrorResponseDto(statusCode, message);
    }

    // Serialisasi ke Camel Case (standar JSON)
    var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });

    context.Response.StatusCode = statusCode;
    await context.Response.WriteAsync(json);
  }
}
