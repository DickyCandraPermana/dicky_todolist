using dicky_todolist.Data;
using dicky_todolist.DTOs.Todo;
using dicky_todolist.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dicky_todolist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _context.Todos
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
            .FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null) return NotFound();

            return Ok(new TodoResponseDto(todo.Id, todo.Title, todo.Description, todo.IsCompleted, todo.CreatedAt));
        }

        [HttpPost]
        public async Task<ActionResult<TodoResponseDto>> CreateTodo(TodoCreateRequestDto request)
        {
            var todo = new Todo
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description ?? "",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            var response = new TodoResponseDto(todo.Id, todo.Title, todo.Description, todo.IsCompleted, todo.CreatedAt);

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(Guid id, TodoUpdateRequestDto request)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null) return NotFound();

            todo.Title = request.Title ?? todo.Title;
            todo.Description = request.Description ?? todo.Description;
            if (request.IsCompleted.HasValue)
            {
                todo.IsCompleted = request.IsCompleted.Value;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteTodo(Guid id)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null) return NotFound();

            todo.IsCompleted = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null) return NotFound();

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
