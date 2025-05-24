using FluentValidation;

namespace PosSystem.Models.Product;

public class ProductRequestValidator:AbstractValidator<ProductRequestModel>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product Name is required!")
            .NotNull().WithMessage("Product Name is required!")
            .MaximumLength(100).WithMessage("Product Name cannot exceed more than 100 characters");

        RuleFor(x => x.ImageName)
            .NotEmpty().WithMessage("Product Image is required!")
            .NotNull().WithMessage("Product Image is required!");

        RuleFor(x => x.Price)
            .NotEqual(0).WithMessage("Price is required!")
            .GreaterThan(0).WithMessage("Price is required!")
            .NotNull().WithMessage("Price is required!");

        RuleFor(x => x.StockQuantity)
            .NotEqual(0).WithMessage("Price is required!")
            .GreaterThan(0).WithMessage("Price is required!")
            .NotNull().WithMessage("Price is required!");

        RuleFor(x => x.CategoryId)
            .NotEqual(0).WithMessage("Please choose category!")
            .NotNull().WithMessage("Please choose category!");
    }
}
