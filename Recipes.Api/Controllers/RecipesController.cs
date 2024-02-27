using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Api.Models.Requests;
using Recipes.Api.Models.Requests.Recipes;
using Recipes.Api.Models.Results;
using Recipes.Core.Application;
using Recipes.Core.Domain;
using Recipes.Core.Infrastructure.Database;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController : ControllerBase
{
    private readonly RecipesDbContext _recipesDbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public RecipesController(RecipesDbContext recipesDbContext, IDateTimeProvider dateTimeProvider, IMapper mapper)
    {
        _recipesDbContext = recipesDbContext;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrUpdateRecipeRequest request, CancellationToken cancellationToken)
    {
        var user = await _recipesDbContext.Users
            .FindAsync(new object[] { User.Identity.Name }, cancellationToken);

        var utcNow = _dateTimeProvider.UtcNow;
        
        var recipe = new Recipe
        {
            Course = request.Course,
            Diet = request.Diet,
            Name = request.Name,
            Instructions = request.Instructions,
            Difficulty = request.Difficulty,
            ApplicationUser = user,
            Created = utcNow,
            LastUpdated = utcNow,
        };

        await _recipesDbContext.AddAsync(recipe, CancellationToken.None);

        await _recipesDbContext.SaveChangesAsync(CancellationToken.None);

        var dto = _mapper.Map<RecipeDto>(recipe);
        
        var url = Url.Action("ReadSingle", new { recipe.Id });

        return Created(url!, dto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ReadSingle(int id, CancellationToken cancellationToken)
    {
        var recipe = await _recipesDbContext.Recipes
            .Include(r => r.ApplicationUser)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        var recipeDto = _mapper.Map<RecipeDto>(recipe);

        return Ok(recipeDto);
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery]GetRecipesRequest request, CancellationToken cancellationToken)
    {
        var skip = (request.Page - 1)  * request.PageSize;

        var recipes = await _recipesDbContext.Recipes
            .Include(r => r.ApplicationUser)
            .Where(r => request.Course == null || r.Course == request.Course)
            .Where(r => request.Diet == null || r.Diet == request.Diet)
            .OrderBy(r => r.Id)
            .Skip(skip)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        var dto = _mapper.Map<IReadOnlyCollection<RecipeDto>>(recipes);

        return Ok(dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CreateOrUpdateRecipeRequest request, CancellationToken cancellationToken)
    {
        var recipe = await _recipesDbContext.Recipes
            .Include(r => r.ApplicationUser)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        
        if (recipe.ApplicationUser.Id != User.Identity.Name)
        {
            throw new UnauthorizedAccessException("User doesn't own recipe.");
        }
        
        recipe.Course = request.Course;
        recipe.Diet = request.Diet;
        recipe.Name = request.Name;
        recipe.Instructions = request.Instructions;
        recipe.Difficulty = request.Difficulty;
        recipe.LastUpdated = _dateTimeProvider.UtcNow;
        
        await _recipesDbContext.SaveChangesAsync(CancellationToken.None);
        
        var dto = _mapper.Map<RecipeDto>(recipe);
        
        return Ok(dto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var recipe = await _recipesDbContext.Recipes
            .Include(r => r.ApplicationUser)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (recipe.ApplicationUser.Id != User.Identity.Name)
        {
            throw new UnauthorizedAccessException("User doesn't own recipe.");
        }

        _recipesDbContext.Recipes.Remove(recipe);

        await _recipesDbContext.SaveChangesAsync(CancellationToken.None);

        return NoContent();
    }
}