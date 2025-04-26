using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Productions.DeleteProductionById;

public sealed record DeleteProductionByIdCommand(Guid Id) : IRequest<Result<string>>;
