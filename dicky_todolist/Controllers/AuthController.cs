using dicky_todolist.Data;
using dicky_todolist.DTOs.Auth;
using dicky_todolist.Services;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password)) return Unauthorized("Email atau password salah!");

            var token = _authService.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }
}
