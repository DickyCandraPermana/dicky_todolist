using dicky_todolist.Data;
using dicky_todolist.DTOs.Todo;
using dicky_todolist.Exceptions;
using dicky_todolist.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dicky_todolist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : BaseApiController
    {
        private readonly AppDbContext _context;
        private readonly IValidator<TodoCreateRequestDto> _createValidator;
        private readonly IValidator<TodoUpdateRequestDto> _updateValidator;

        public TodosController(
            AppDbContext context,
            IValidator<TodoCreateRequestDto> createValidator,
            IValidator<TodoUpdateRequestDto> updateValidator
            )
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _context.Todos
            .Where(t => t.UserId == GetUserId())
            .Select(t => new TodoResponseDto(
                t.Id,
                t.Title,
                t.Description,
                t.IsCompleted,
                t.CreatedAt))
            .ToListAsync();

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo(Guid id)
        {
            var todo = await _context.Todos
            .Where(t => t.UserId == GetUserId())
            .FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null) throw new TodoNotFoundException("Todo tidak ditemukan");

            return Ok(new TodoResponseDto(todo.Id, todo.Title, todo.Description, todo.IsCompleted, todo.CreatedAt));
        }

        [HttpPost]
        public async Task<ActionResult<TodoResponseDto>> CreateTodo(TodoCreateRequestDto request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var todo = new Todo
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description ?? "",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                UserId = GetUserId()
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            var response = new TodoResponseDto(todo.Id, todo.Title, todo.Description, todo.IsCompleted, todo.CreatedAt);

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(Guid id, TodoUpdateRequestDto request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

            if (todo == null) throw new TodoNotFoundException("Todo tidak ditemukan");

            todo.Title = request.Title ?? todo.Title;
            todo.Description = request.Description ?? todo.Description;
            if (request.IsCompleted.HasValue)
            {
                todo.IsCompleted = request.IsCompleted.Value;
            }
            todo.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new TodoResponseDto(todo.Id, todo.Title, todo.Description, todo.IsCompleted, todo.CreatedAt));
        }

        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> CompleteTodo(Guid id)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

            if (todo == null) throw new TodoNotFoundException("Todo tidak ditemukan");

            todo.IsCompleted = true;
            todo.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new TodoResponseDto(todo.Id, todo.Title, todo.Description, todo.IsCompleted, todo.CreatedAt));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

            if (todo == null) throw new TodoNotFoundException("Todo tidak ditemukan");

            todo.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
