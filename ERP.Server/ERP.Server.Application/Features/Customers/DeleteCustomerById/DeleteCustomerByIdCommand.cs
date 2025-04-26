using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Customers.DeleteCustomerById;

public sealed record DeleteCustomerByIdCommand(Guid Id) : IRequest<Result<string>>;
