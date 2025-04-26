using ERP.Server.Domain.Dtos;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Enums;
using ERP.Server.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Features.Orders.RequirementsPlanningByOrderId;

public sealed record RequirementsPlanningByOrderIdCommand(Guid OrderId) : IRequest<Result<RequirementsPlanningByOrderIdCommandResponse>>;

public sealed record RequirementsPlanningByOrderIdCommandResponse(
    DateOnly Date,
    string Title,
    List<ProductDto> Products,
    bool hasRecipe);
internal sealed class RequirementsPlanningByOrderIdCommandHandler(
    IOrderRepository orderRepository,
    IStockMovementRepository stockMovementRepository,
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RequirementsPlanningByOrderIdCommand, Result<RequirementsPlanningByOrderIdCommandResponse>>
{
    public async Task<Result<RequirementsPlanningByOrderIdCommandResponse>> Handle(RequirementsPlanningByOrderIdCommand request, CancellationToken cancellationToken)
    {
        bool hasRecipe = true;  // Varsayılan olarak reçete var kabul edelim.

        Order? order =
            await orderRepository
            .Where(p => p.Id == request.OrderId)
            .Include(p => p.Details!)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return Result<RequirementsPlanningByOrderIdCommandResponse>.Failure("Sipariş bulunamadı");
        }

        List<ProductDto> uretilmesiGerekenUrunListesi = new();
        List<ProductDto> requirementsPlanningProducts = new();



        if (order.Details is not null)
        {
            foreach (var item in order.Details)
            {

                var product = item.Product;
                List<StockMovement> movements =
                    await stockMovementRepository
                    .Where(p => p.ProductId == product!.Id)
                    .ToListAsync(cancellationToken);

                decimal stock = movements.Sum(p => p.NumberOfEntries) - movements.Sum(p => p.NumberOfOutputs);

                if (stock < item.Quantity)
                {
                    ProductDto uretilmesiGerekenUrun = new()
                    {
                        Id = item.ProductId,
                        Name = product!.Name,
                        Quantity = item.Quantity - stock
                    };

                    uretilmesiGerekenUrunListesi.Add(uretilmesiGerekenUrun);
                }
            }



            foreach (var item in uretilmesiGerekenUrunListesi)
            {
                Recipe? recipe =
                    await recipeRepository
                    .Where(p => p.ProductId == item.Id)
                    .Include(p => p.Details!)
                    .ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(cancellationToken);

                if (recipe is null)
                {
                    hasRecipe = false; // ✅ Eğer ürünün reçetesi yoksa, hasRecipe değerini false yap.
                    continue;  // ❗ `return Failure(...)` yerine, döngüye devam et.
                }

                if (recipe is not null)
                {
                    if (recipe.Details is not null)
                    {
                        foreach (var detail in recipe.Details)
                        {
                            List<StockMovement> urunMovements =
                                    await stockMovementRepository
                                    .Where(p => p.ProductId == detail.ProductId)
                                    .ToListAsync(cancellationToken);

                            decimal stock = urunMovements.Sum(p => p.NumberOfEntries) - urunMovements.Sum(p => p.NumberOfOutputs);

                            if (stock < detail.Quantity)
                            {
                                ProductDto ihtiyacOlanUrun = new()
                                {
                                    Id = detail.ProductId,
                                    Name = detail.Product!.Name,
                                    Quantity = detail.Quantity - stock
                                };

                                requirementsPlanningProducts.Add(ihtiyacOlanUrun);
                                hasRecipe = false; // Eğer stok yetersizse bunu false yapıyoruz!
                            }
                        }
                    }

                }
                //else
                //{
                //    return Result<RequirementsPlanningByOrderIdCommandResponse>.Failure("Ürünün reçetesi bulunamadı. İhtiyaç planlaması yapılamaz.");
                //}
            }
        }

        //Siparişteki ürünlerin tüm depolardaki adetlerine bakacağım
        //eğer yetersiz ise kaç tane üretilmesi gerektiğine bakacağım
        //her bir ürün için gereken ürün reçetesini alacağım ve o ürünlerin tek tek stoklarına bakacağım
        //üretilmesi için gereken ürünlerden kaç tanesine ihtiyacımız olduğunu tespit edip liste olarak geri döndüreceğiz


        //sipariş ihtiyaç planlamasında aynı ürünleri gruplamak için yazdık bu kodu
        requirementsPlanningProducts = requirementsPlanningProducts
            .GroupBy(p => p.Id)
            .Select(g => new ProductDto
            {
                Id = g.Key,
                Name = g.First().Name,
                Quantity = g.Sum(item => item.Quantity)
            }).ToList();

        //Sipariş durumu için ihtiyaç planı çalışıldı olarak değiştirdik 
        order.Status = OrderStatusEnum.RequirementsPlanWorked;
        orderRepository.Update(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RequirementsPlanningByOrderIdCommandResponse(
            DateOnly.FromDateTime(DateTime.Now),
            order.Number + "Nolu Siparişin İhtiyaç Planlaması",
            requirementsPlanningProducts,
            hasRecipe);
    }
}
