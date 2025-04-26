using AutoMapper;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.Features.Orders.CreateOrder;

internal sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateOrderCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Order? LastOrder = await orderRepository.Where(p => p.OrderNumberYear == request.Date.Year).OrderByDescending(p => p.OrderNumber).FirstOrDefaultAsync(cancellationToken);
        int lastOrderNumber = 0;

        if (LastOrder is not null) lastOrderNumber = LastOrder.OrderNumber;

        //List<OrderDetail> details = request.Details.Select(s => new OrderDetail
        //{
        //    Price = s.Price,
        //    ProductId = s.ProductId,
        //    Quantity = s.Quantity,
        //}).ToList();

        //Order order = new()
        //{
        //    CustomerId = request.CustomerId,
        //    Date = request.Date,
        //    DeliveryDate = request.DeliveryDate,
        //    OrderNumber = lastOrderNumber++,
        //    OrderNumberYear = request.Date.Year,
        //    Details = details
        //};

        //eğer mapper kullanırsak yukarıdaki nesne oluşturmalarına gerek kalmaz
        Order order = mapper.Map<Order>(request);
        order.OrderNumber = lastOrderNumber + 1;
        order.OrderNumberYear = request.Date.Year;

        await orderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Sipariş başarıyla oluşturuldu";
    }
}
