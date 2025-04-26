using ERP.Server.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.RecipeDetails.GetRecipeByIdWithDetails;

public sealed record GetRecipeByIdWithDetailsQuery(Guid RecipeId) : IRequest<Result<Recipe>>;
