using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Enums;
using ERP.Server.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Features.Invoices.GetAllInvoice;

internal sealed class GetAllInvoiceQueryHandler(
    IInvoiceRepository ınvoiceRepository) : IRequestHandler<GetAllInvoiceQuery, Result<List<Invoice>>>
{
    public async Task<Result<List<Invoice>>> Handle(GetAllInvoiceQuery request, CancellationToken cancellationToken)
    {
        List<Invoice> ınvoices = await ınvoiceRepository
            .Where(p => p.Type == InvoiceTypeEnum.FromValue(request.Type))
            .Include(p => p.Customer)
            .Include(p => p.Details!)
            .ThenInclude(p => p.Product)
            .Include(p => p.Details!)
            .ThenInclude(p => p.Depot)
            .OrderBy(p => p.Date)
            .ToListAsync(cancellationToken);

        return ınvoices;
    }
}
