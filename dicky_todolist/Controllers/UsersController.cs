using dicky_todolist.Data;
using dicky_todolist.DTOs.User;
using dicky_todolist.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dicky_todolist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
            .Select(u => new UserResponseDTO(
                u.Id,
                u.Username,
                u.Email,
                u.CreatedAt,
                u.UpdatedAt,
                u.DeletedAt
            ))
            .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            return Ok(new UserResponseDTO(
                user.Id,
                user.Username,
                user.Email,
                user.CreatedAt,
                user.UpdatedAt,
                user.DeletedAt
            ));
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> Store(UserCreateRequestDto request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response = new UserResponseDTO
            (
                user.Id,
                user.Username,
                user.Email,
                user.CreatedAt,
                user.UpdatedAt,
                user.DeletedAt
            );

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, response);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UserResponseDTO>> Update(UserUpdateRequestDto request, Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(request.Username)) user.Username = request.Username;
            if (!string.IsNullOrWhiteSpace(request.Email)) user.Email = request.Email;
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
            }

            await _context.SaveChangesAsync();

            var response = new UserResponseDTO(
                user.Id,
                user.Username,
                user.Email,
                user.CreatedAt,
                user.UpdatedAt,
                user.DeletedAt
            );

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            user.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
