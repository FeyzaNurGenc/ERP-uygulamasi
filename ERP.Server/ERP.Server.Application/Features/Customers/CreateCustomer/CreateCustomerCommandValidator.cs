using FluentValidation;

namespace ERP.Server.Application.Features.Customers.CreateCustomer;

public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(p => p.TaxNumber).MinimumLength(3).MaximumLength(11);
        RuleFor(p => p.Name).MinimumLength(3);
    }
}
