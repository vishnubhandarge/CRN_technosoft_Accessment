using System.Threading.Tasks;
using Application.DTOs.Auth;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequestDto request);
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, string ipAddress);
    Task<LoginResponseDto?> RefreshTokenAsync(string token, string ipAddress);
    Task<bool> RevokeTokenAsync(string token, string ipAddress);
}
