using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.RecipeDetails.DeleteRecipeDetailById;

public sealed record DeleteRecipeDetailByIdCommand(Guid Id) : IRequest<Result<string>>;
