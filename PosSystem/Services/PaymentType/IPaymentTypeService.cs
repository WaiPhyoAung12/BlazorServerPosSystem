using PosSystem.Models.PaymentType;
using PosSystem.Models.Shared;

namespace PosSystem.Services.PaymentType;

public interface IPaymentTypeService
{
    Task<Result<PaymentTypeModelList>> GetPaymentTypesByPaymentMethod(int paymentMethodId);
}
