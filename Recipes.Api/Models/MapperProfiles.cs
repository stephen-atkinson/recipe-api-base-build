using AutoMapper;
using Recipes.Api.Models.Dtos;
using Recipes.Core.Domain;

namespace Recipes.Api.Models;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        CreateMap<Recipe, RecipeDto>();
    }
}