using System.Security.Claims;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.Authenticate;
using LibraSoft.Core.Requests.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Signup(CreateUserRequest req, IUserHandler handler)
        {
            var exists = await handler.GetByEmailOrTelephoneAsync(req.Email, req.Telephone);

            if(exists?.Email == req.Email)
            {
                return BadRequest(new UserAlreadyExistsError());
            }

            if (exists?.Telephone == req.Telephone)
            {
                return BadRequest(new UserTelephoneAlreadyExists());
            }
            
            await handler.CreateAsync(req);

            return Created();
        }

        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Signin(SigninUserRequest req, IUserHandler handler, ITokenClaimsService tokenService)
        {
            var request = new GetByEmailRequest { Email = req.Email };
            var user = await handler.GetByEmailAsync(request);

            if (user is null)
            {
                return BadRequest(new UserOrPasswordIncorrectError());
            }

            var verifyPassword = BCrypt.Net.BCrypt.Verify(req.Password, user.Password);

            if (!verifyPassword)
            {
                return BadRequest(new UserOrPasswordIncorrectError());
            }

            var tokens = tokenService.GetTokens(user);

            return Ok(new { tokens.access_token, tokens.refresh_token });
        }

        [HttpPost("refresh_token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest req, IUserHandler handler, ITokenClaimsService tokenService)
        {
            var principal = tokenService.GetPrincipalFromToken(req.Refresh_Token);

            var id = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var user = await handler.GetByIdAsync(Guid.Parse(id));

            if(user is null)
            {
                return BadRequest();
            }

            var tokens = tokenService.GetNewAccessToken(user, req.Refresh_Token);

            if (tokens is null)
            {
                return BadRequest();
            }

            return Ok(new { tokens.access_token, tokens.refresh_token });
        }

        [HttpPost("signup/adm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignupAdministrator(CreateUserRequest req, IUserHandler handler)
        {
            var request = new GetByEmailRequest { Email = req.Email };

            var exists = await handler.GetByEmailAsync(request);

            if (exists is not null)
            {
                return BadRequest(new UserAlreadyExistsError());
            }

            await handler.CreateAsync(req, isAdmin: true);

            return Created();
        }

        [HttpGet("check")]
        [Authorize]
        public IActionResult CheckAuthentication()
        {
            return Ok();
        }
    }
}
