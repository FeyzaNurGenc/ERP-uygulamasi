using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.RecipeDetails.CreateRecipeDetail;

public sealed record CreateRecipeDetailCommand(
    Guid RecipeId,
    Guid ProductId,
    decimal Quantity) : IRequest<Result<string>>;
