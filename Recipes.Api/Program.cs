using Microsoft.AspNetCore.Authentication.JwtBearer;
using Recipes.Api.Options;
using Recipes.Core.Application;
using Recipes.Core.Infrastructure;
using Recipes.Core.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.ConfigureOptions<JwtConfigOptions>();

builder.Services.AddControllers();
builder.Services.ConfigureOptions<RouteConfigOptions>();

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<SwaggerConfigOptions>();
builder.Services.ConfigureOptions<SwaggerUiConfigOptions>();

builder.Services.AddApiVersioning().AddApiExplorer();
builder.Services.ConfigureOptions<ApiVersioningConfigOptions>();
builder.Services.ConfigureOptions<ApiExplorerConfigOptions>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<RecipesDbContext>();

var app = builder.Build();

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
public partial class Program { }