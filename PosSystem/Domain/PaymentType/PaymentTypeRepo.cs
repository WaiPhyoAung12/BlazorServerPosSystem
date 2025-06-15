using Databases.AppDbContextModels;
using Mapster;
using Microsoft.EntityFrameworkCore;
using PosSystem.Models.Payment;
using PosSystem.Models.PaymentType;
using PosSystem.Models.Shared;

namespace PosSystem.Domain.PaymentType;

public class PaymentTypeRepo
{
    private readonly AppDbContext _appDbContext;

    public PaymentTypeRepo(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Result<PaymentTypeModelList>>GetPaymentTypesByPaymentMethod(int paymentMethod)
    {
        //if (paymentMethod is 0)
        //    return Result<PaymentTypeModelList>.Fail("Bad Request");

        var paymentTypeList=await _appDbContext.TblPaymentTypes.Where(x=>x.PaymentMethod == paymentMethod).ToListAsync();

        if (paymentTypeList.Count == 0)
            Result<PaymentTypeModelList>.Fail("Data Not Found");

        PaymentTypeModelList paymentTypeModelList = new();
        paymentTypeModelList.PaymentTypeModels= paymentTypeList.Adapt<List<PaymentTypeModel>>();

        return Result<PaymentTypeModelList>.Success("Success", paymentTypeModelList);
    }
}
