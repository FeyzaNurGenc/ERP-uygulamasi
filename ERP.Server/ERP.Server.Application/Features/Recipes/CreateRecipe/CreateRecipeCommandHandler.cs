using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Recipes.CreateRecipe;

internal sealed class CreateRecipeCommandHandler(
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateRecipeCommand, Result<string>>
{
    //burada farklı ayarlama yapmak için mapper kullanmadık
    public async Task<Result<string>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        bool isRecipeExists = await recipeRepository.AnyAsync(p => p.ProductId == request.ProductId, cancellationToken);
        if (isRecipeExists)
        {
            return Result<string>.Failure("Bu ürüne ait reçete daha önce oluşturulmuş");
        }


        //EN ÖNEMLİ KISIM BURASI
        //burada Command da verdiğimiz parametreyi RecipeDetail clasına eşitliyoruz
        Recipe recipe = new()
        {
            ProductId = request.ProductId,
            Details = request.Details.Select(s =>
            new RecipeDetail()
            {
                ProductId = s.ProductId,
                Quantity = s.Quantity,
            }).ToList(),
        };

        await recipeRepository.AddAsync(recipe);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Reçete kaydı başarıyla tamamlandı";

    }
}
