using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft. EntityFrameworkCore;
using JwtAuthApi.Data;
using JwtAuthApi.Services;
using JwtAuthApi.Models;


namespace JwtAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        private readonly PasswordHasher<string> _hasher = new();

        public AuthController(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (await _context.Users.AnyAsync(x => x.Email == model.Email))
                return BadRequest("Email already exists.");

            var user = new User
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PasswordHash = _hasher.HashPassword(model.Email, model.Password),
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Registered successfully.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = _hasher.VerifyHashedPassword(model.Email, user.PasswordHash, model.Password);
            if (result != PasswordVerificationResult.Success)
                return Unauthorized("Invalid credentials");

            var accessToken = _tokenService.CreateAccessToken(user);
            var refreshToken = _tokenService.CreateRefreshToken(user);

            return Ok(new
            {
                token = accessToken,
                refreshToken = refreshToken.Token
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var token = await _context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(x => x.Token == refreshToken && !x.IsRevoked && x.Expires > DateTime.UtcNow);

            token.IsRevoked = true;
            await _context.SaveChangesAsync();

            var accessToken = _tokenService.CreateAccessToken(token.User);

            var newRefreshToken = _tokenService.CreateRefreshToken(token.User);

            return Ok(new
            {
                token = accessToken,
                refreshToken = newRefreshToken.Token
            });
        }

    }
}
