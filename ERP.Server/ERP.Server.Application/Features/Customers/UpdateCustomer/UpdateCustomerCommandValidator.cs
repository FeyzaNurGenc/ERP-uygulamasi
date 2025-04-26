using FluentValidation;

namespace ERP.Server.Application.Features.Customers.UpdateCustomer;

public sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(p => p.TaxNumber).MinimumLength(3).MaximumLength(11);
    }
}
