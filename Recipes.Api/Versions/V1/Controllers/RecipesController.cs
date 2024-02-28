using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Recipes.Api.Versions.V1.Models.Dtos;
using Recipes.Api.Versions.V1.Models.Requests.Recipes;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Models;
using Recipes.Core.Domain;

namespace Recipes.Api.Versions.V1.Controllers;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
public class RecipesController : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateOrUpdateRecipeRequest> _validator;

    public RecipesController(IRecipeRepository recipeRepository, IMapper mapper, IValidator<CreateOrUpdateRecipeRequest> validator)
    {
        _recipeRepository = recipeRepository;
        _mapper = mapper;
        _validator = validator;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrUpdateRecipeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return ValidationProblem(ModelState);
        }
        
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

        if (recipe == null)
        {
            return NotFound();
        }

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
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return ValidationProblem(ModelState);
        }
        
        var recipe = await _recipeRepository.GetAsync(id, cancellationToken);

        if (recipe == null)
        {
            return NotFound();
        }
        
        if (recipe.UserId != User.Identity.Name)
        {
            return Unauthorized();
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
        
        if (recipe == null)
        {
            return NotFound();
        }
        
        if (recipe.UserId != User.Identity.Name)
        {
            return Unauthorized();
        }

        await _recipeRepository.DeleteAsync(id, CancellationToken.None);

        return NoContent();
    }

    [HttpPatch("{id:int}/rating")]
    public async Task<IActionResult> CreateOrUpdateRating(int id, CreateOrUpdateRecipeRatingRequest request, CancellationToken cancellationToken)
    {
        var recipe = await _recipeRepository.GetAsync(id, cancellationToken);
        
        if (recipe == null)
        {
            return NotFound();
        }

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