using ERP.Server.Domain.Entities;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Invoices.GetAllInvoice;

public sealed record GetAllInvoiceQuery(
    int Type) : IRequest<Result<List<Invoice>>>;
