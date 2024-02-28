using Microsoft.AspNetCore.Authentication.JwtBearer;
using Recipes.Api.HealthChecks;
using Recipes.Api.Options;
using Recipes.Core.Application.Extensions;
using Recipes.Core.Infrastructure.Database;
using Recipes.Core.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.ConfigureOptions<JwtConfigOptions>();

builder.Services.AddControllers();
builder.Services.ConfigureOptions<MvcConfigOptions>();
builder.Services.ConfigureOptions<RouteConfigOptions>();
builder.Services.ConfigureOptions<JsonConfigOptions>();

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<MvcConfigOptions>();
builder.Services.ConfigureOptions<SwaggerConfigOptions>();
builder.Services.ConfigureOptions<SwaggerUiConfigOptions>();

builder.Services.AddApiVersioning().AddApiExplorer();
builder.Services.ConfigureOptions<ApiVersioningConfigOptions>();
builder.Services.ConfigureOptions<ApiExplorerConfigOptions>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<RecipesDbContext>()
    .AddCheck<IngredientsApiHealthCheck>(IngredientsApiHealthCheck.HealthCheckName);

var app = builder.Build();

await app.EnsureDatabase();
await app.SeedUsers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();

// Make the implicit Program class public so test projects can access it
namespace Recipes.Api
{
    public partial class Program { }
}