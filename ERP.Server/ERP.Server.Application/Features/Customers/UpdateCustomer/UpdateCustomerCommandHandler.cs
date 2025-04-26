using AutoMapper;
using ERP.Server.Domain.Entities;
using ERP.Server.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Customers.UpdateCustomer;

internal sealed class UpdateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateCustomerCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer customer = await customerRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (customer == null)
        {
            return Result<string>.Failure("Müşteri bulunamadı");
        }

        if (customer.TaxNumber != request.TaxNumber)
        {
            bool isTaxNumberExists = await customerRepository.AnyAsync(p => p.TaxNumber == request.TaxNumber, cancellationToken);
            if (isTaxNumberExists)
            {
                return Result<string>.Failure("Vergi numarası daha önce kaydedilmiş");
            }

        }
        mapper.Map(request, customer);
        // customerRepository.Update(customer); GetByExpressionWithTrackingAsync kullanıldığı için repository için Update etmeye ihtiyaç kalmadı
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Müşteri bilgileri başarıyla güncellendi";

    }
}
