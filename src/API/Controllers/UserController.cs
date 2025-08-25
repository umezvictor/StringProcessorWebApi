using Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared;
using Shared.Requests;
using Shared.Responses;


namespace Webly.Controllers
{

    [EnableRateLimiting(Constants.RateLimitingPolicy)]
    public class UserController(IAuthService authService) : BaseController
    {

        [HttpPost("login")]
        [ProducesResponseType(typeof(Result<AuthenticationResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
        {
            var response = await authService.Login(request);
            if (response.IsFailure)
                return BadRequest(response.Error);

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(Result<RefreshTokenResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
        {
            var response = await authService.GenerateRefreshTokenAsync(request);
            if (response.IsFailure)
                return BadRequest(response.Error);
            return Ok(response);
        }

    }
}
