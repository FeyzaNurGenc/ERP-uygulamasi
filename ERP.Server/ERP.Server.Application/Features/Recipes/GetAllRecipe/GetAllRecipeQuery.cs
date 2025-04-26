using ERP.Server.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Recipes.GetAllRecipe;

public sealed record GetAllRecipeQuery : IRequest<Result<List<Recipe>>>;
