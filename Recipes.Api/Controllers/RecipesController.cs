using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipes.Api.Models.Dtos;
using Recipes.Api.Models.Requests.Recipes;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Models;
using Recipes.Core.Domain;

namespace Recipes.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public RecipesController(IRecipeRepository recipeRepository, IDateTimeProvider dateTimeProvider, IMapper mapper)
    {
        _recipeRepository = recipeRepository;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrUpdateRecipeRequest request, CancellationToken cancellationToken)
    {
        var utcNow = _dateTimeProvider.UtcNow;
        
        var recipe = new Recipe
        {
            Course = request.Course,
            Diet = request.Diet,
            Name = request.Name,
            Instructions = request.Instructions,
            Difficulty = request.Difficulty,
            UserId = User.Identity.Name,
            Created = utcNow,
            LastUpdated = utcNow,
        };

        recipe.Id = await _recipeRepository.CreateAsync(recipe, CancellationToken.None);

        var dto = _mapper.Map<RecipeDto>(recipe);
        
        var url = Url.Action("ReadSingle", new { recipe.Id });

        return Created(url!, dto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ReadSingle(int id, CancellationToken cancellationToken)
    {
        var recipe = await _recipeRepository.GetAsync(id, cancellationToken);

        var recipeDto = _mapper.Map<RecipeDto>(recipe);

        return Ok(recipeDto);
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery]GetRecipesRequest request, CancellationToken cancellationToken)
    {
        var skip = (request.Page - 1)  * request.PageSize;

        var searchCriteria = new GetRecipesCriteria
        {
            Course = request.Course,
            Diet = request.Diet,
            Skip = skip,
            Take = request.PageSize,
            DifficultyFrom = request.DifficultyFrom,
            DifficultyTo = request.DifficultyTo
        };

        var recipes = await _recipeRepository.GetAsync(searchCriteria, cancellationToken);

        var dto = _mapper.Map<IReadOnlyCollection<RecipeDto>>(recipes);

        return Ok(dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CreateOrUpdateRecipeRequest request, CancellationToken cancellationToken)
    {
        var recipe = await _recipeRepository.GetAsync(id, cancellationToken);
        
        if (recipe.UserId != User.Identity.Name)
        {
            throw new UnauthorizedAccessException("User doesn't own recipe.");
        }
        
        recipe.Course = request.Course;
        recipe.Diet = request.Diet;
        recipe.Name = request.Name;
        recipe.Instructions = request.Instructions;
        recipe.Difficulty = request.Difficulty;
        recipe.LastUpdated = _dateTimeProvider.UtcNow;

        await _recipeRepository.UpdateAsync(recipe, CancellationToken.None);
        
        var dto = _mapper.Map<RecipeDto>(recipe);
        
        return Ok(dto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var recipe = await _recipeRepository.GetAsync(id, cancellationToken);
        
        if (recipe.UserId != User.Identity.Name)
        {
            throw new UnauthorizedAccessException("User doesn't own recipe.");
        }

        await _recipeRepository.DeleteAsync(id, CancellationToken.None);

        return NoContent();
    }
}