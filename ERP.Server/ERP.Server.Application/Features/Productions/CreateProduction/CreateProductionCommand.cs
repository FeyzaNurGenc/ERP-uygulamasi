using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Productions.CreateProduction;

public sealed record CreateProductionCommand(
    Guid ProductId,
    Guid DepotId,
    decimal Quantity
    ) : IRequest<Result<string>>;
