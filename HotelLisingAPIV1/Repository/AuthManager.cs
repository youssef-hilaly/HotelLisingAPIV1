using AutoMapper;
using HotelLisingAPIV1.DTOs.Auth;
using HotelLisingAPIV1.DTOs.Login;
using HotelLisingAPIV1.Interfaces;
using HotelLisingAPIV1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelLisingAPIV1.Repository
{
    public class AuthManager : IAuthManager
    {
        private const string _loginProvider = "HotelListingApi";
        private const string _refreshToken = "RefreshToken";

        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager; // default library help with the registration
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken); // remove previous token
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken); // create new one
            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken); // set it in the DB

            return newRefreshToken;

        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            _user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (_user == null)
            {
                return null;
            }
            
            var validPassword = await _userManager.CheckPasswordAsync(_user, loginDto.Password);
            if (!validPassword)
            {
                return null;
            }

            return new AuthResponseDto { UserId = _user.Id, Token = await GenerateToken(), RefreshToken = await CreateRefreshToken() };
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = _user.Email;

            IdentityResult result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                if (_user.Email.Contains("Admin"))
                    await _userManager.AddToRoleAsync(_user, "Admin");
                else
                    await _userManager.AddToRoleAsync(_user, "User");
            }

            return result.Errors;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token); // get the token

            var userEmail = tokenContent.Claims.ToList().FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value; // find the user email in the token

            _user = await _userManager.FindByEmailAsync(userEmail); // get the user

            if (_user == null || _user.Id != request.UserId) return null; 

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken); 

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto { UserId = _user.Id, Token = token, RefreshToken = await CreateRefreshToken() };
            }

            await _userManager.UpdateSecurityStampAsync(_user); //sign out any saved login for the user
            return null;
        }

        private async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // token credentials

            var roles = await _userManager.GetRolesAsync(_user); // get user role
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
            
            //var userClaims = _userManager.GetClaimsAsync(user); // if there were any claims

            var claims = new List<Claim> 
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.UserName), // subject or the person (Username)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // prevent playback attackes so the value changes every time
                new Claim(JwtRegisteredClaimNames.Email, _user.Email), // 
                new Claim("uid", _user.Id)
            }.Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(
                    _configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
