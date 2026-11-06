using InvoiceTracker.Domain.Interfaces.Services;
using InvoiceTracker.Domain.Models.Authentication;
using InvoiceTracker.Domain.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserService _userService;

    public AccountController(
        IUserService userService,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager)
    {
        _userService = userService;
        _logger = logger;
        _roleManager = roleManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest register,
        CancellationToken cancellationToken = default)
    {
        await _userService.Register(register, cancellationToken);

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest login,
        CancellationToken cancellationToken = default)
    {
        var result = await _userService.LoginRequest(login, cancellationToken);
        if (result is null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        return Ok(new
        {
            accessToken = result.AccessToken,
            expiresAt = result.ExpiresAt,
            roles = result.Roles
        });
    }

    [HttpPost("add-role")]
   
    public async Task<IActionResult> AddRole([FromBody] string role)
    {
        if (await _roleManager.RoleExistsAsync(role))
        {
            return BadRequest(new { message = "Role already exists" });
        }

        var result = await _roleManager.CreateAsync(new IdentityRole(role));
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = "Role created successfully" });
    }
}