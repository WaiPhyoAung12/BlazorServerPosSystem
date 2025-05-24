using FluentValidation;

namespace PosSystem.Models.SaleDetails;

public class SaleDetailsModelValidator:AbstractValidator<SaleDetailsModel>
{
    public SaleDetailsModelValidator()
    {
        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price is required");

        RuleFor(x => x.SaleId)
            .NotEqual(0)
            .WithMessage("Sale id is required");

        RuleFor(x => x.SubTotal)
            .GreaterThan(0)
            .WithMessage("Sub total is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity is required");

        RuleFor(x => x.ProductId)
            .NotEqual(0)
            .WithMessage("Product id is required");

    }
}
