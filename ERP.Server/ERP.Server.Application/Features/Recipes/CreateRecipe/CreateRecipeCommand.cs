using ERP.Server.Domain.Dtos;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Recipes.CreateRecipe;

public sealed record CreateRecipeCommand(
    Guid ProductId,
    List<RecipeDetailDto> Details) : IRequest<Result<string>>;
