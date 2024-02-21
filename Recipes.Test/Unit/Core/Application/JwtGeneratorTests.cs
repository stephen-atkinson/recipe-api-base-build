using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Options;
using Moq;
using Recipes.Core.Application;
using Recipes.Core.Application.Auth;
using Recipes.Core.Domain;

namespace Recipes.Test.Unit.Core.Application;

[TestFixture]
public class JwtGeneratorTests
{
    private MockRepository _mockRepository;
    
    private Mock<IOptionsMonitor<JwtSettings>> _mockJwtOptions;
    private Mock<IDateTimeProvider> _mockDateTimeProvider;
    
    private ApplicationUser _applicationUser;
    private JwtSettings _jwtSettings;
    private DateTime _utcNow;
    private DateTime _validTo;
    
    private JwtGenerator _jwtGenerator;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new MockRepository(MockBehavior.Strict);

        _mockJwtOptions = _mockRepository.Create<IOptionsMonitor<JwtSettings>>();
        _mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();

        _applicationUser = new ApplicationUser { Id = "02381b52-371e-4577-9dde-912296904466" };
        
        _jwtSettings = new JwtSettings
        {
            Audience = nameof(Api),
            Issuer = nameof(Core),
            ExpiresIn = TimeSpan.FromDays(1),
            SigningKey = "DontUseYourRealKeyHereKeepThatSecure"
        };
        
        _utcNow = DateTime.UtcNow.At(9, 30, 15); // Clear the milliseconds, as JWT uses seconds.
        _validTo = _utcNow.Add(_jwtSettings.ExpiresIn);
        
        _jwtGenerator = new JwtGenerator(_mockJwtOptions.Object, _mockDateTimeProvider.Object);
    }

    [Test]
    public void Create_SetsCorrectProperties()
    {
        // Arrange
        
        _mockDateTimeProvider.SetupGet(p => p.UtcNow).Returns(_utcNow);
        _mockJwtOptions.SetupGet(o => o.CurrentValue).Returns(_jwtSettings);

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        
        // Act
        
        var jwt = _jwtGenerator.Create(_applicationUser);
        var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwt);

        // Assert

        jwtSecurityToken.Claims.Should().Contain(c => c.Value == _applicationUser.Id);
        jwtSecurityToken.Issuer.Should().Be(_jwtSettings.Issuer);
        jwtSecurityToken.Audiences.Should().ContainSingle(_jwtSettings.Audience);
        jwtSecurityToken.IssuedAt.Should().Be(_utcNow);
        jwtSecurityToken.ValidTo.Should().Be(_validTo);
    }
}