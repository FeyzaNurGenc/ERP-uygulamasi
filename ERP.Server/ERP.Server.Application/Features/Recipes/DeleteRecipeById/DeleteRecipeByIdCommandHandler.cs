using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Recipes.DeleteRecipeById;

internal sealed class DeleteRecipeByIdCommandHandler(
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteRecipeByIdCommand, Result<string>>
{
    async Task<Result<string>> IRequestHandler<DeleteRecipeByIdCommand, Result<string>>.Handle(DeleteRecipeByIdCommand request, CancellationToken cancellationToken)
    {
        Recipe recipe = await recipeRepository.GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);

        recipeRepository.Delete(recipe);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Ürün reçetesi başarıyla silindi";
    }
}
