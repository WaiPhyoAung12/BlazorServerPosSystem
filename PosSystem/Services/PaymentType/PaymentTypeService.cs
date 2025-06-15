using PosSystem.Domain.PaymentType;
using PosSystem.Models.PaymentType;
using PosSystem.Models.Shared;

namespace PosSystem.Services.PaymentType;

public class PaymentTypeService:IPaymentTypeService
{
    private readonly PaymentTypeRepo _paymentTypeRepo;

    public PaymentTypeService(PaymentTypeRepo paymentTypeRepo)
    {
        _paymentTypeRepo = paymentTypeRepo;
    }
    public async Task<Result<PaymentTypeModelList>> GetPaymentTypesByPaymentMethod(int paymentMethodId)
    {
        var result=await _paymentTypeRepo.GetPaymentTypesByPaymentMethod(paymentMethodId);
        return result;
    }
}
