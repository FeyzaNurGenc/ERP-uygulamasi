using ERP.Server.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Orders.GetAllOrder;

public sealed record GetAllOrderQuery() : IRequest<Result<List<Order>>>;
