using FluentValidation;

namespace PosSystem.Models.Sale;

public class SaleRequestValidator:AbstractValidator<SaleRequestModel>
{
    public SaleRequestValidator()
    {
        RuleFor(x => x.TotalAmount)
            .GreaterThan(0)
            .WithMessage("Total amount is required");

        RuleFor(x => x.TotalCount)
            .GreaterThan(0)
            .WithMessage("Total count is required");

        RuleFor(x => x.PaymentType)
            .GreaterThan(0)
            .WithMessage("Payment type is required");

        RuleFor(x => x.SaleDetailsList.Count)
            .GreaterThan(0)
            .WithMessage("Sale details list is required");

    }
}
