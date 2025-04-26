using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Orders.DeleteOrderById;

internal sealed record DeleteOrderByIdCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteOrderByIdCommand request, CancellationToken cancellationToken)
    {
        Order order = await orderRepository.GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);
        if (order is null)
        {
            return Result<string>.Failure("Sipariş bulunamadı");
        }

        orderRepository.Delete(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Sipariş başarıyla silindi";
    }
}
