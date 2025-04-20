using FluentValidation;

namespace PosSystem.Models.Category;

public class CategoryRequestValidator:AbstractValidator<CategoryRequestModel>
{
    public CategoryRequestValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Category Name is required!")
            .NotNull().WithMessage("Category Name is required!")
            .MaximumLength(100).WithMessage("Category Name cannot exceed more than 100 characters");

        RuleFor(x => x.ImageName)
            .NotEmpty().WithMessage("Image Name is required!")
            .NotNull().WithMessage("Image Name is required!")
            .MaximumLength(100).WithMessage("Image Name cannot exceed more than 100 characters");
    }
}
