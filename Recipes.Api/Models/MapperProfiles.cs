using AutoMapper;
using Recipes.Api.Models.Results;
using Recipes.Core.Domain;

namespace Recipes.Api.Models;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        CreateMap<Recipe, RecipeDto>()
            .ForMember(r => r.CreatedBy, c => c.MapFrom(r => r.ApplicationUser.UserName));
    }
}