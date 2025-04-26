using AutoMapper;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Enums;
using ERP.Server.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Features.Invoices.CreateInvoice;

internal sealed record CreateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IStockMovementRepository StockMovementRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateInvoiceCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        Invoice invoice = mapper.Map<Invoice>(request);

        //eğer bu ürün varsa stok hareketleri olarak eklememiz lazım
        if (invoice.Details is not null)
        {
            List<StockMovement> movements = new();
            foreach (var item in invoice.Details)
            {
                // Satış faturasıysa, stok kontrolü yapalım
                if (request.TypeValue == 2) // Satış faturası
                {
                    var stockMovement = await StockMovementRepository.Where(
                        sm => sm.ProductId == item.ProductId).ToListAsync();

                    // Eğer stok hareketi varsa, mevcut stok miktarını hesapla
                    decimal stock = stockMovement.Sum(p => p.NumberOfEntries) - stockMovement.Sum(p => p.NumberOfOutputs);

                    // Yeterli stok olup olmadığını kontrol et
                    if (stock < item.Quantity)
                    {
                        return Result<string>.Failure($"Ürün {item.ProductId} için yeterli stok bulunmamaktadır.");
                    }
                }

                StockMovement movement = new()
                {
                    InvoiceId = invoice.Id,
                    NumberOfEntries = request.TypeValue == 1 ? item.Quantity : 0,
                    NumberOfOutputs = request.TypeValue == 2 ? item.Quantity : 0,
                    DepotId = item.DepotId,
                    Price = item.Price,
                    ProductId = item.ProductId
                };
                movements.Add(movement);
            }
            await StockMovementRepository.AddRangeAsync(movements, cancellationToken);
        }

        await invoiceRepository.AddAsync(invoice, cancellationToken);

        if (request.OrderId is not null)
        {
            Order order = await orderRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.OrderId);

            if (order != null)
            {
                order.Status = OrderStatusEnum.Completed;
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Fatura başarıyla oluşturuldu";
    }
}
