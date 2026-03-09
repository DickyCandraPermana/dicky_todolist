using dicky_todolist.Data;
using dicky_todolist.DTOs.User;
using dicky_todolist.Exceptions;
using dicky_todolist.Models;
using FluentValidation;
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
        private readonly IValidator<UserCreateRequestDto> _createValidator;
        private readonly IValidator<UserUpdateRequestDto> _updateValidator;

        public UsersController(
            AppDbContext context,
            IValidator<UserCreateRequestDto> createValidator,
            IValidator<UserUpdateRequestDto> updateValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
            .Select(u => new UserResponseDto(
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new UserNotFoundException("Pengguna tidak ditemukan!");

            return Ok(new UserResponseDto(
                      user.Id,
                      user.Username,
                      user.Email,
                      user.CreatedAt,
                      user.UpdatedAt,
                      user.DeletedAt
                  ));
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Store(UserCreateRequestDto request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                throw new BadHttpRequestException("Username sudah digunakan");
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new BadHttpRequestException("Email sudah digunakan");
            }

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

            var response = new UserResponseDto
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
        public async Task<ActionResult<UserResponseDto>> Update(UserUpdateRequestDto request, Guid id)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new UserNotFoundException("User tidak ditemukan");

            if (!string.IsNullOrWhiteSpace(request.Username)) user.Username = request.Username;
            if (!string.IsNullOrWhiteSpace(request.Email)) user.Email = request.Email;
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
            }

            await _context.SaveChangesAsync();

            var response = new UserResponseDto(
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new UserNotFoundException("User tidak ditemukan");

            user.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
