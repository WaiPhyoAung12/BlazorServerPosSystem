using FluentValidation;

namespace PosSystem.Models.Payment;

public class PaymentModelValidator:AbstractValidator<PaymentModel>
{
    public PaymentModelValidator()
    {
        RuleFor(x => x.PaymentType)
            .NotEqual(0)
            .WithMessage("Payment type is required");

        RuleFor(x=>x.SaleId)
            .NotEqual(0)
            .WithMessage("Sale id is required");

        RuleFor(x => x.AmountPaid)
            .GreaterThan(0)
            .WithMessage("Amount is required");
    }
}
