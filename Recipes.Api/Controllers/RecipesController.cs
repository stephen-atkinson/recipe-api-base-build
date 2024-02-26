using Microsoft.AspNetCore.Mvc;
using Recipes.Api.Models.Requests;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateRecipeRequest request)
    {
        
    }
}