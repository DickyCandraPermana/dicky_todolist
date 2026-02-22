using dicky_todolist.Data;
using dicky_todolist.DTOs.Auth;
using dicky_todolist.DTOs.User;
using dicky_todolist.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dicky_todolist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("askskkask/{id}")]
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


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password)) return Unauthorized("Email atau password salah!");

            var token = _authService.GenerateToken(user);

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateRequestDto request, [FromServices] IValidator<UserCreateRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                // Middleware kita akan menangkap ini dan mengubahnya jadi JSON yang rapi
                throw new ValidationException(validationResult.Errors);
            }

            var user = new Models.User
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
    }
}
