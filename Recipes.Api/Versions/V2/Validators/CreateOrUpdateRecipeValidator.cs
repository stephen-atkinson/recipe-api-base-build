using FluentValidation;
using Recipes.Api.Versions.V2.Models.Requests.Recipes;

namespace Recipes.Api.Versions.V2.Validators;

public class CreateOrUpdateRecipeValidator : AbstractValidator<CreateOrUpdateRecipeRequest>
{
    public CreateOrUpdateRecipeValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Instructions).MaximumLength(500);
        RuleFor(r => r.Difficulty).InclusiveBetween(1, 5);
        RuleFor(r => r.Course).IsInEnum();
        RuleFor(r => r.Diet).IsInEnum();
    }
}