using TaskManagementApi.DTOs.Auth;

namespace TaskManagementApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
}