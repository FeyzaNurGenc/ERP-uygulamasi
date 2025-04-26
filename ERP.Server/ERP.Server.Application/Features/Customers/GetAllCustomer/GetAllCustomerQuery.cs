using ERP.Server.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Customers.GetAllCustomer;

public sealed record GetAllCustomerQuery : IRequest<Result<List<Customer>>>;
