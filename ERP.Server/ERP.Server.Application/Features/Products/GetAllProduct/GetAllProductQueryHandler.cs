using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Features.Products.GetAllProduct;

internal sealed class GetAllProductQueryHandler(
    IProductRepository productRepository,
    IStockMovementRepository stockMovementRepository) : IRequestHandler<GetAllProductQuery, Result<List<GetAllProductQueryResponse>>>
{
    public async Task<Result<List<GetAllProductQueryResponse>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        List<Product> products = await productRepository.GetAll().OrderBy(p => p.Name).ToListAsync(cancellationToken);

        List<GetAllProductQueryResponse> response = products.Select(s => new GetAllProductQueryResponse
        {
            Id = s.Id,
            Name = s.Name,
            Type = s.Type,
            Stock = 0
        }).ToList();

        foreach (var item in response)
        {
            decimal stock = await stockMovementRepository.Where(p => p.ProductId == item.Id).SumAsync(s => s.NumberOfEntries - s.NumberOfOutputs);
            item.Stock = stock;
        }
        return response;
    }
}
