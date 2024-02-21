using Microsoft.AspNetCore.Authentication.JwtBearer;
using Recipes.Api.Options;
using Recipes.Core.Application;
using Recipes.Core.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer()
    .Services
    .ConfigureOptions<JwtConfigOptions>();

builder.Services.AddControllers();

builder.Services.Configure<RouteOptions>(o => { o.LowercaseUrls = true; });

builder.Services.AddSwaggerGen()
    .ConfigureOptions<SwaggerConfigOptions>()
    .ConfigureOptions<SwaggerUiConfigOptions>();

builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    // ReSharper disable once StringLiteralTypo
    options.GroupNameFormat = "'v'VVV";
});

var app = builder.Build();

await app.SeedUsers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }