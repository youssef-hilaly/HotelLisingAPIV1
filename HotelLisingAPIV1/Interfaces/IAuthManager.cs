using HotelLisingAPIV1.DTOs.Auth;
using HotelLisingAPIV1.DTOs.Login;
using HotelLisingAPIV1.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelLisingAPIV1.Interfaces
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto user);
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<string> CreateRefreshToken();
        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);
    }
}
