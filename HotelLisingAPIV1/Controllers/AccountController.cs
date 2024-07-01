using HotelLisingAPIV1.DTOs.Auth;
using HotelLisingAPIV1.DTOs.Login;
using HotelLisingAPIV1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HotelLisingAPIV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AccountController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        // api/account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            var errors = await _authManager.Register(apiUserDto);

            return errors.Any()? BadRequest(errors) : Created("register", new {message =  "Account is created Successfully" });

            //another way
            //foreach (var error in errors)
            //  ModelState.AddModelError(error.Code, error.Description);
            //return BadRequest(ModelState);
        }


        // api/account/register
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            AuthResponseDto authResponse = await _authManager.Login(loginDto);

            return authResponse == null ? Unauthorized("Invalid Email or Password") : Ok(authResponse);
        }

        // api/account/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto authResponseDto)
        {
            AuthResponseDto authResponse = await _authManager.VerifyRefreshToken(authResponseDto);

            return authResponse == null ? Unauthorized() : Ok(authResponse);
        }





    }
}
