using AutoMapper;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Features.Productions.CreateProduction;

internal sealed class CreateProductionCommandHandler(
    IProductionRepository productionRepository,
    IRecipeRepository recipeRepository,
    IStockMovementRepository stockMovementRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateProductionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateProductionCommand request, CancellationToken cancellationToken)
    {
        /*CreateProductionCommand sadece sade veriler içerir(ProductId, DepotId, Quantity gibi).
        Ama biz bunu veritabanına yazmak için Production türüne çevirdik.*/

        Production production = mapper.Map<Production>(request);

        /*Üretim yapılırken bazı hammaddeler azalır(örneğin un, şeker).
        Ve aynı zamanda yeni ürün artmış olur.
        Bu stok hareketlerini burada toplayacağız.*/

        List<StockMovement> newMovements = new();

        /*“Bu ürün hangi hammaddelerle üretiliyor? Reçetesini bul.”
        Include ve ThenInclude sayesinde ilişkili tablolar da çekiliyor*/

        Recipe? recipe = await recipeRepository.Where(p => p.ProductId == request.ProductId).Include(p => p.Details!).ThenInclude(p => p.Product).FirstOrDefaultAsync(cancellationToken);


        // Null kontrolü, Eğer ürünün reçetesi hiç tanımlanmamışsa, üretim yapılamaz.
        if (recipe == null)
        {
            return Result<string>.Failure("Ürüne ait bir reçete bulunamadı. Üretim gerçekleştirilemez.");
        }

        //Hem reçete varsa hem de reçetenin detayları(yani hangi hammadde, ne kadar kullanılacak bilgisi) varsa devam ediyoruz.
        if (recipe is not null && recipe.Details is not null)
        {
            /*Her bir reçete maddesi için döngüye giriyoruz.
            (Örnek: Kek üretmek için 2 yumurta, 1 su bardağı süt gibi)
            item.ProductId: Bu, gereken hammadde.
            O hammaddenin sistemdeki tüm stok hareketlerini alıyoruz.
            (Yani geçmişte ne kadar giriş yapılmış, ne kadar çıkış olmuş)*/

            var details = recipe.Details;
            foreach (var item in details)
            {
                List<StockMovement> movements = await stockMovementRepository.Where(p => p.ProductId == item.ProductId).ToListAsync(cancellationToken);


                /*Bu hammaddenin bulunduğu depo ID'lerini çıkarıyoruz.
                Çünkü hammadde birden fazla depoda olabilir.
                (Örneğin, 50 kg un ana depoda, 20 kg üretim alanında olabilir.)*/

                List<Guid> depotIds = movements.GroupBy(p => p.DepotId).Select(g => g.Key).ToList();

                //Bu hammaddenin toplam stok miktarı hesaplanıyor.NumberOf stok giriş/ çıkış miktarlarını tutan property.
                decimal stok = movements.Sum(p => p.NumberOfEntries) - movements.Sum(p => p.NumberOfOutputs);

                //Eğer stok yetmiyorsa, üretim yapamıyoruz ve kullanıcıya hangi ürün eksik ve ne kadar eksik olduğunu gösteriyoruz.
                if (item.Quantity > stok)
                {
                    return Result<string>.Failure(item.Product!.Name + "ürününden üretim için yeterli miktarda yok. Eksik miktar: " +
                        (item.Quantity - stok));
                }

                /*Ürünün bağlı olduğu tüm depoları tek tek dolaşıyoruz.
                  Amaç: Stok ihtiyacını, depoların sırasına göre kapatmak.
                  Eğer bir depodan gerekli miktarın tamamını bulursak diğer depolara gerek kalmadan çıkıyoruz (break).*/
                foreach (var depotId in depotIds)
                {
                    if (item.Quantity <= 0) break;

                    //Bu depoda gerçekte kalan stok ne kadar? Girişler - Çıkışlar ile bulunur.
                    decimal quantity = movements.Where(p => p.DepotId == depotId).Sum(s => s.NumberOfEntries - s.NumberOfOutputs);

                    decimal totalAmount = movements.Where(p => p.DepotId == depotId && p.NumberOfEntries > 0).Sum(s => s.Price * s.NumberOfEntries);

                    decimal totalEntriesQuantity = movements.Where(p => p.DepotId == depotId && p.NumberOfEntries > 0).Sum(s => s.NumberOfEntries);

                    decimal price = totalAmount / totalEntriesQuantity;

                    /*Burada yeni bir stok hareketi oluşturuyorsun ve:
                      Üretime ait.
                      Hangi ürün, hangi depodan çıktı belli.
                      Çıkış fiyatı da belli.*/
                    StockMovement stockMovement = new()
                    {
                        ProductionId = production.Id,
                        ProductId = item.ProductId,
                        DepotId = depotId,
                        Price = price
                    };

                    /*Eğer ihtiyacımız item.Quantity, eldeki stok quantity den küçükse tamamını buradan al.
                    Yetmezse, alabildiğin kadar al ve kalan eksik miktar için bir sonraki depoya gitmek üzere döngüye devam et.*/
                    if (item.Quantity <= quantity)
                    {
                        stockMovement.NumberOfOutputs = item.Quantity;
                    }
                    else
                    {
                        stockMovement.NumberOfOutputs = quantity;
                    }
                    item.Quantity -= quantity;
                    /*Bu kod sayesinde sistem, ihtiyacı birden fazla depoya yayarak karşılamaya çalışıyor.
                     Yani "tek depoda yoksa, diğer depolardan da al" diyor.*/
                    newMovements.Add(stockMovement);
                }
            }
        }

        await stockMovementRepository.AddRangeAsync(newMovements, cancellationToken);
        await productionRepository.AddAsync(production, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Ürün başarıyla üretildi";
    }
}
