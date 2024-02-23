using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Recipes.Api.Models.Requests;
using Recipes.Core.Application;
using Recipes.Core.Infrastructure.Database;

namespace Recipes.Test.Integration.Controllers;

public class AuthControllerTests
{
    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _httpClient;

    [SetUp]
    public void SetUp()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b =>
            {
                b.UseEnvironment("Test");
                b.ConfigureServices(sc =>
                {
                    sc.Replace(ServiceDescriptor
                        .Singleton<IRecipesDbContextOptionsFactory, TestRecipesDbContextOptionsFactory>());
                });
            });
        
        _httpClient = _webApplicationFactory.CreateClient();
    }

    [Test]
    public async Task Index_AuthenticatesUser()
    {
        // Arrange
        var userSettings = _webApplicationFactory.Services
            .GetRequiredService<IOptions<UserSettings>>();
        
        var defaultUser = userSettings.Value.DefaultUsers.First();

        var request = new LoginRequest
        {
            Username = defaultUser.Username,
            Password = defaultUser.Password
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/auth", request, default);
        var result = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNullOrWhiteSpace();
    }
    
    [TearDown]
    public void TearDown()
    {
        _webApplicationFactory.Dispose();
    }
}