using InvoiceTracker.Api.Controllers;
using InvoiceTracker.Domain.Interfaces.Services;
using InvoiceTracker.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InvoiceTracker.UnitTests;

public sealed class AccountControllerTests
{
    [Fact]
    public async Task Login_ReturnsOk_WhenCredentialsAreValid()
    {
        var userService = new Mock<IUserService>();
        var expected = new LoginResponse(
            "access-token",
            "token",
            [],
            DateTime.UtcNow.AddMinutes(60));
        userService
            .Setup(service => service.LoginRequest(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = CreateController(userService.Object);
        var result = await controller.Login(
            new LoginRequest("mphomoaga@gmail.com", "Password1!", "Email"));

        var response = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, response.StatusCode);
        userService.Verify(service => service.LoginRequest(
            It.Is<LoginRequest>(request => request.Email == "mphomoaga@gmail.com"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
    {
        var userService = new Mock<IUserService>();
        userService
            .Setup(service => service.LoginRequest(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResponse)null!);

        var controller = CreateController(userService.Object);
        var result = await controller.Login(
            new LoginRequest("mphomoaga@gmail.com", "Password1d", "Email"));

        var response = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal(401, response.StatusCode);
    }

    private static AccountController CreateController(IUserService userService)
    {
        var roleStore = new Mock<IRoleStore<IdentityRole>>();
        var roleManager = new RoleManager<IdentityRole>(
            roleStore.Object,
            [],
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            Mock.Of<ILogger<RoleManager<IdentityRole>>>());

        return new AccountController(
            userService,
            Mock.Of<ILogger<AccountController>>(),
            roleManager);
    }
}
