using dicky_todolist.Data;
using dicky_todolist.DTOs.Auth;
using dicky_todolist.DTOs.User;
using dicky_todolist.Exceptions;
using dicky_todolist.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dicky_todolist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IValidator<UserCreateRequestDto> _createValidator;
        private readonly IValidator<LoginRequestDto> _loginValidator;

        public AuthController(
            AppDbContext context,
            IAuthService authService,
            IValidator<UserCreateRequestDto> createValidator,
            IValidator<LoginRequestDto> loginValidator)
        {
            _context = context;
            _authService = authService;
            _createValidator = createValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password))
            {
                throw new BadHttpRequestException("Email atau password salah!");
            }

            var token = _authService.GenerateToken(user);

            var userResponse = new UserResponseDto
            (
                user.Id,
                user.Username,
                user.Email,
                user.CreatedAt,
                user.UpdatedAt,
                user.DeletedAt
            );

            return Ok(new { Token = token, User = userResponse });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserCreateRequestDto request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                // Middleware kita akan menangkap ini dan mengubahnya jadi JSON yang rapi
                throw new ValidationException(validationResult.Errors);
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new BadHttpRequestException("Email sudah digunakan");
            }

            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                throw new BadHttpRequestException("Username sudah digunakan");
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

            var response = new UserResponseDto
            (
                user.Id,
                user.Username,
                user.Email,
                user.CreatedAt,
                user.UpdatedAt,
                user.DeletedAt
            );

            return CreatedAtAction(
                "GetById",
                "Users",
                new { id = user.Id },
                response
            );
        }

        [HttpPost("profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _context.Users.Where(u => u.Id == GetUserId()).FirstOrDefaultAsync() ?? throw new UserNotFoundException("Pengguna tidak ditemukan!");

            return Ok(new UserResponseDto(
                      user.Id,
                      user.Username,
                      user.Email,
                      user.CreatedAt,
                      user.UpdatedAt,
                      user.DeletedAt
                  ));
        }
    }
}
