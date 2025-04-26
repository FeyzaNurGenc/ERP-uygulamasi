using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Orders.DeleteOrderById;

public sealed record DeleteOrderByIdCommand(Guid Id) : IRequest<Result<string>>;
