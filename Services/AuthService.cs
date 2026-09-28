using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs.Auth;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.login_id == request.loginId &&
                    u.password == request.password);

            if (user == null)
            {
                return null;
            }

            return new LoginResponseDto
            {
                userId = user.user_id,
                loginId = user.login_id,
                name = user.name,
                designation = user.designation,
                role = user.Role!.role_name
            };
        }
    }
}