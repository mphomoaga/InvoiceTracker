using Azure;
using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(
            [FromBody] LoginRequest loginRequest,
            CancellationToken cancellationToken)
        {
            var response = await _authService.LoginRequest(loginRequest, cancellationToken);

            if (!response.LoginSuccess)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserRequest registerUser,
            CancellationToken cancellationToken)
        {
            var response = await _authService.Register(registerUser, cancellationToken);

            if (!response.Success)
            {
                return Unauthorized(response); ;
            }

            return StatusCode(StatusCodes.Status201Created, new
            {
                response
            });
        }
    }
}
