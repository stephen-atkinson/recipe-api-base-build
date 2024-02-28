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
    private readonly IMapper _mapper;

    public RecipesController(IRecipeRepository recipeRepository, IMapper mapper)
    {
        _recipeRepository = recipeRepository;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrUpdateRecipeRequest request, CancellationToken cancellationToken)
    {
        var recipe = MapRequestToRecipe(null, request);

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
            DifficultyTo = request.DifficultyTo,
            UserId = request.UserId
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
        
        recipe = MapRequestToRecipe(recipe, request);

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

    [HttpPatch("{id:int}/rating")]
    public async Task<IActionResult> CreateOrUpdateRating(int id, CreateOrUpdateRecipeRatingRequest request, CancellationToken cancellationToken)
    {
        var recipe = await _recipeRepository.GetAsync(id, cancellationToken);

        var rating = recipe.Ratings.FirstOrDefault(r => r.UserId == User.Identity.Name);

        if (rating == null)
        {
            recipe.Ratings.Add(rating = new Rating { UserId = User.Identity.Name });
        }

        rating.Value = request.Rating;

        await _recipeRepository.UpdateAsync(recipe, CancellationToken.None);

        return NoContent();
    }

    private Recipe MapRequestToRecipe(Recipe? recipe, CreateOrUpdateRecipeRequest request)
    {
        recipe ??= new Recipe
        {
            Ingredients = new List<Ingredient>(),
            Ratings = new List<Rating>(),
            UserId = User.Identity.Name
        };

        recipe.Course = request.Course;
        recipe.Diet = request.Diet;
        recipe.Name = request.Name;
        recipe.Instructions = request.Instructions;
        recipe.Difficulty = request.Difficulty;
        
        return recipe;
    }
}