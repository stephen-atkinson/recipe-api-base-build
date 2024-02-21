using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Recipes.Api.Controllers;
using Recipes.Api.Models.Requests;
using Recipes.Api.Models.Results;
using Recipes.Core.Application.Auth;
using Recipes.Core.Domain;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Recipes.Test.Unit.Api.Controllers;

public class AuthControllerTests
{
    private Mock<IAccessTokenGenerator> _mockAccessTokenGenerator;
    private Mock<AspNetUserManager<ApplicationUser>> _mockAspNetUserManager;
    private Mock<SignInManager<ApplicationUser>> _mockSignInManager;

    private AuthController _authController;

    private LoginRequest _request;
    private ApplicationUser _applicationUser;

    [SetUp]
    public void SetUp()
    {
        _mockAccessTokenGenerator = new Mock<IAccessTokenGenerator>();
        
        _mockAspNetUserManager = new Mock<AspNetUserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), 
            null, null, null, null, null, null, null, null);
        
        _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
            _mockAspNetUserManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            null, null, null, null);

        _authController = new AuthController(
            _mockAccessTokenGenerator.Object,
            _mockAspNetUserManager.Object,
            _mockSignInManager.Object);
        
        _request = new LoginRequest { Username = "joe.bloggs" };
        _applicationUser = new ApplicationUser();
    }
    
    [Test]
    public async Task Index_ReturnsUnauthorized_WhenUserNull()
    {
        // Arrange

        _mockAspNetUserManager
            .Setup(m => m.FindByNameAsync(_request.Username))
            .ReturnsAsync((ApplicationUser)null!);

        // Act

        var result = await _authController.Index(_request, default);
        
        // Assert

        result.Should().BeOfType<UnauthorizedResult>();
    }
    
    [Test]
    public async Task Index_ReturnsUnauthorized_WhenPasswordCheckFails()
    {
        // Arrange
        
        _mockAspNetUserManager
            .Setup(m => m.FindByNameAsync(_request.Username))
            .ReturnsAsync(_applicationUser);

        _mockSignInManager
            .Setup(m => m.CheckPasswordSignInAsync(_applicationUser, _request.Password, false))
            .ReturnsAsync(SignInResult.Failed);
        
        // Act

        var result = await _authController.Index(_request, default);
        
        // Assert

        result.Should().BeOfType<UnauthorizedResult>();
    }
    
    [Test]
    public async Task Index_ReturnsAccessToken_WhenAuthenticated()
    {
        // Arrange

        const string token = "ABCD";
        
        _mockAspNetUserManager
            .Setup(m => m.FindByNameAsync(_request.Username))
            .ReturnsAsync(_applicationUser);

        _mockSignInManager
            .Setup(m => m.CheckPasswordSignInAsync(_applicationUser, _request.Password, false))
            .ReturnsAsync(SignInResult.Success);

        _mockAccessTokenGenerator
            .Setup(g => g.Create(_applicationUser))
            .Returns(token);
        
        // Act

        var result = await _authController.Index(_request, CancellationToken.None);
        
        // Assert

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<LoginResult>()
            .Which.AccessToken.Should().Be(token);
    }
}