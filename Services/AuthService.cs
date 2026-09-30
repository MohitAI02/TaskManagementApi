using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs.Auth;
using TaskManagementApi.Models;
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
             User? user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.LoginId == request.LoginId &&
                    u.Password == request.Password);

            if (user == null)
            {
                return null;
            }

            return new LoginResponseDto
            {
                UserId = user.UserId,
                LoginId = user.LoginId,
                Name = user.Name,
                Designation = user.Designation,
                Role = user.Role!.RoleName
            };
        }
    }
}