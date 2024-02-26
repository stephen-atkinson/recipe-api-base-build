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

        CreateMap<Group, GroupDto>()
            .ForMember(g => g.CreatedBy, c => c.MapFrom(g => g.ApplicationUser.UserName))
            .ForMember(g => g.RecipeIds, c => c.MapFrom(g => g.Recipes.Select(r => r.Id).ToArray()));
    }
}