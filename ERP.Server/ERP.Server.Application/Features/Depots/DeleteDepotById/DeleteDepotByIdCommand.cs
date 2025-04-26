using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Depots.DeleteDepotById;

public sealed record DeleteDepotByIdCommand(Guid Id) : IRequest<Result<string>>;
