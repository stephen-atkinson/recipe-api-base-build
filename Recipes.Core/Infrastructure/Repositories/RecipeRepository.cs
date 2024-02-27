using Microsoft.EntityFrameworkCore;
using Recipes.Core.Application.Contracts;
using Recipes.Core.Application.Models;
using Recipes.Core.Domain;

namespace Recipes.Core.Infrastructure.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly IRecipesDbContext _recipesDbContext;

    public RecipeRepository(IRecipesDbContext recipesDbContext)
    {
        _recipesDbContext = recipesDbContext;
    }

    public async Task<int> CreateAsync(Recipe recipe, CancellationToken cancellationToken)
    {
        var dbRecipe = new Recipe();

        Map(recipe, dbRecipe);

        await _recipesDbContext.Recipes.AddAsync(dbRecipe, cancellationToken);
        await _recipesDbContext.SaveChangesAsync(cancellationToken);

        return dbRecipe.Id;
    }

    public async Task UpdateAsync(Recipe recipe, CancellationToken cancellationToken)
    {
        var dbRecipe = await _recipesDbContext.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Ratings)
            .FirstOrDefaultAsync(r => r.Id == recipe.Id, cancellationToken);

        if (dbRecipe == null)
        {
            throw new NullReferenceException($"No recipe found with id \"{recipe.Id}\".");
        }

        Map(recipe, dbRecipe);

        await _recipesDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var recipe = await _recipesDbContext.Recipes.FindAsync(new object[] { id }, cancellationToken);

        if (recipe == null)
        {
            throw new NullReferenceException($"No recipe found with id \"{id}\".");
        }

        _recipesDbContext.Recipes.Remove(recipe);

        await _recipesDbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<Recipe?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var recipe = await _recipesDbContext.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .Include(r => r.Ratings)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return recipe;
    }

    public async Task<IReadOnlyCollection<Recipe>> GetAsync(GetRecipesCriteria criteria,
        CancellationToken cancellationToken)
    {
        var recipes = await _recipesDbContext.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .Include(r => r.Ratings)
            .Where(r => criteria.Course == null || r.Course == criteria.Course)
            .Where(r => criteria.Diet == null || r.Diet == criteria.Diet)
            .Where(r => criteria.Ids == null || criteria.Ids.Contains(r.Id))
            .Where(r => criteria.DifficultyFrom == null || r.Difficulty >= criteria.DifficultyFrom)
            .Where(r => criteria.DifficultyTo == null || r.Difficulty <= criteria.DifficultyTo)
            .Where(r => criteria.UserId == null || r.UserId == criteria.UserId)
            .OrderBy(r => r.Name)
            .Skip(criteria.Skip)
            .Take(criteria.Take)
            .ToArrayAsync(cancellationToken);

        return recipes;
    }

    private static void Map(Recipe from, Recipe to)
    {
        to.Course = from.Course;
        to.Diet = from.Diet;
        to.Name = from.Name;
        to.Instructions = from.Instructions;
        to.Difficulty = from.Difficulty;
        to.UserId = from.UserId;

        to.Ingredients = from.Ingredients.Select(fi =>
        {
            var ingredient = to.Ingredients.FirstOrDefault(i => i.ExternalId == fi.ExternalId) ?? new Ingredient();

            ingredient.Name = fi.Name;
            ingredient.Category = fi.Category;
            ingredient.Description = fi.Description;
            ingredient.ExternalId = fi.ExternalId;
            ingredient.SupplierName = fi.SupplierName;

            return ingredient;

        }).ToList();

        to.Ratings = from.Ratings.Select(fr =>
        {
            var rating = to.Ratings.FirstOrDefault(r => r.UserId == fr.UserId) ?? new Rating();
            
            rating.UserId = fr.UserId;
            rating.Value = fr.Value;

            return rating;
        }).ToList();
    }
}