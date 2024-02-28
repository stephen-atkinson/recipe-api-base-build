using AutoMapper;
using Recipes.Api.Versions.V2.Models.Dtos;
using Recipes.Core.Domain;

namespace Recipes.Api.Versions.V2.Models;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        CreateMap<Recipe, RecipeDto>()
            .ForMember(r => r.AverageRating, c => c.MapFrom(r => r.Ratings.Select(r => r.Value).DefaultIfEmpty().Average()));

        CreateMap<Ingredient, IngredientDto>();
    }
}