using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Recipes.Api.Versions.V2.Models.Requests.Recipes;
using Recipes.Api.Versions.V2.Models.Dtos;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Models;
using Recipes.Core.Domain;

namespace Recipes.Api.Versions.V2.Controllers;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[ApiVersion(2)]
public class RecipesController : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateOrUpdateRecipeRequest> _validator;
    private readonly IIngredientsApi _ingredientsApi;

    public RecipesController(IRecipeRepository recipeRepository, IMapper mapper, IValidator<CreateOrUpdateRecipeRequest> validator, IIngredientsApi ingredientsApi)
    {
        _recipeRepository = recipeRepository;
        _mapper = mapper;
        _validator = validator;
        _ingredientsApi = ingredientsApi;
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
        
        var recipe = await RequestToRecipeAsync(null, request, cancellationToken);

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
        
        recipe = await RequestToRecipeAsync(id, request, cancellationToken);

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

    private async Task<Recipe> RequestToRecipeAsync(int? id, CreateOrUpdateRecipeRequest request, CancellationToken cancellationToken)
    {
        var batchGetIngredientsRequest = new BatchGetIngredientsRequest { Ids = request.IngredientIds };
        
        var ingredients = await _ingredientsApi.BatchGet(batchGetIngredientsRequest, cancellationToken);
        
        var recipe = new Recipe
        {
            Id = id ?? 0,
            Course = request.Course,
            Diet = request.Diet,
            Name = request.Name,
            Instructions = request.Instructions,
            Difficulty = request.Difficulty,
            UserId = User.Identity.Name,
            Ingredients = ingredients.Select(i => new Ingredient
            {
                Name = i.SupplierFriendlyName,
                Category = i.Category,
                Description = i.Description,
                ExternalId = i.Id,
                SupplierName = i.SupplierFriendlyName
            }).ToList(),
        };

        return recipe;
    }
}