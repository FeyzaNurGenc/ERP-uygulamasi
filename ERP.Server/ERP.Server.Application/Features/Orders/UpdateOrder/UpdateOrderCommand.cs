using ERP.Server.Domain.Dtos;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Orders.UpdateOrder;

public sealed record UpdateOrderCommand(
    Guid Id,
    Guid customerId,
    DateOnly Date,
    DateOnly DeliveryDate,
    List<OrderDetailDto> Details) : IRequest<Result<string>>;
