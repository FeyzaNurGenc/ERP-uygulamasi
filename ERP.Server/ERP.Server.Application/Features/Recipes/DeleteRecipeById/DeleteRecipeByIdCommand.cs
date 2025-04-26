using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Recipes.DeleteRecipeById;

public sealed record DeleteRecipeByIdCommand(Guid Id) : IRequest<Result<string>>;
