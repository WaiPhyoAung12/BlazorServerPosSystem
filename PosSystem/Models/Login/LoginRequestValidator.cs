using FluentValidation;

namespace PosSystem.Models.Login
{
    public class LoginRequestValidator:AbstractValidator<LoginRequestModel>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Invalid user name")
                .NotNull().WithMessage("Invalid user name")
                .Length(3, 50).WithMessage("Üser name must between 3 and 50 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Invalid password")
                .NotNull().WithMessage("Invalid password");
        }
    }
}
