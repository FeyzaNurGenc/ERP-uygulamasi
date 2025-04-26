using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Invoices.DeleteInvoiceById;

public sealed record DeleteInvoiceByIdCommand(Guid Id) : IRequest<Result<string>>;
