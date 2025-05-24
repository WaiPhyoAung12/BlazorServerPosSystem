using Databases.AppDbContextModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PosSystem.Models.Category;
using PosSystem.Models.Payment;
using PosSystem.Models.Sale;
using PosSystem.Models.SaleDetails;
using PosSystem.Models.Shared;
using PosSystem.Services.Shared;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace PosSystem.Domain.Sale;

public class SaleRepo
{
    private readonly HttpContextService _httpContextService;
    private readonly AppDbContext _appDbContext;

    public SaleRepo(HttpContextService httpContextService,AppDbContext appDbContext)
    {
        _httpContextService = httpContextService;
        _appDbContext = appDbContext;
    }
    public async Task<Result<SaleResponseModel>> PurchaseProduct(SaleRequestModel saleRequestModel)
    {
        using var transcope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        int saleId;

        SaleRequestValidator saleRequestValidator = new();
        var validationResult= saleRequestValidator.Validate(saleRequestModel);

        if (!validationResult.IsValid)
            return Result<SaleResponseModel>.FailValidation(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

        SaleModel saleModel = new()
        {
            TotalAmount = saleRequestModel.TotalAmount,
        };
        var saleCreateResult=await SaleCreate(saleModel);
        saleId = saleCreateResult.Data.Id;
        if(saleCreateResult.IsError)
            return Result<SaleResponseModel>.Fail(saleCreateResult.Message);

        PaymentModel paymentModel = new()
        {
            SaleId=saleId,
            PaymentType=saleRequestModel.PaymentType,
            AmountPaid=saleRequestModel.AmountPaid,
            ChangeGiven=saleRequestModel.Changes 
        };
        var paymentCreateResult=await CreatePayment(paymentModel);
        if(paymentCreateResult.IsError)
            return Result<SaleResponseModel>.Fail(saleCreateResult.Message);

        var saleDetailsCreateResult = await SaleDetailsListCreate(saleRequestModel.SaleDetailsList,saleId);
        if (saleDetailsCreateResult.IsError)
            return Result<SaleResponseModel>.Fail(saleDetailsCreateResult.Message);

        transcope.Complete();
        return Result<SaleResponseModel>.Success("Success");

    }

    private async Task<Result<PaymentModel>> CreatePayment(PaymentModel paymentModel)
    {
        try
        {
            PaymentModelValidator paymentModelValidator = new();
            var validationResult = paymentModelValidator.Validate(paymentModel);

            if (!validationResult.IsValid)
                return Result<PaymentModel>.FailValidation(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

            TblPayment tblPayment = paymentModel.Adapt<TblPayment>();
            tblPayment.CreatedDateTime=DateTime.Now;
            tblPayment.CreatedUserId = _httpContextService.UserId;
            await _appDbContext.TblPayments.AddAsync(tblPayment);
            var result = await _appDbContext.SaveChangesAsync();

            return result <= 0
                ? Result<PaymentModel>.Fail("Error Create")
                : Result<PaymentModel>.Success("Success Create");
        }
        catch (Exception e)
        {

            throw;
        }
    }

    private async Task<Result<SaleModel>> SaleCreate(SaleModel saleModel)
    {

        TblSale tblSale = saleModel.Adapt<TblSale>();
        tblSale.SaleDate = DateTime.Now;
        tblSale.CreatedUserId=_httpContextService.UserId;
        
        await _appDbContext.TblSales.AddAsync(tblSale);
        var result = await _appDbContext.SaveChangesAsync();

        saleModel.Id = tblSale.Id;

        return result <= 0
            ? Result<SaleModel>.Fail("Error Create")
            : Result<SaleModel>.Success("Success Create",saleModel);

    }

    private async Task<Result<SaleDetailsModel>>SaleDetailsListCreate(List<SaleDetailsModel> saleDetailsList,int saleId)
    {
        foreach(var saleDetails in saleDetailsList)
        {
            saleDetails.SaleId=saleId;
            var result=await SaleDetailsCreate(saleDetails);
            if (result.IsError)
                return result;
        }
        return Result<SaleDetailsModel>.Success("Success Create");
    }

    private async Task<Result<SaleDetailsModel>> SaleDetailsCreate(SaleDetailsModel saleDetailsModel)
    {
        SaleDetailsModelValidator saleDetailsValidator = new();
        var validationResult = saleDetailsValidator.Validate(saleDetailsModel);

        if (!validationResult.IsValid)
            return Result<SaleDetailsModel>.FailValidation(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

        TblSaleDetail tblSaleDetail=saleDetailsModel.Adapt<TblSaleDetail>();
        tblSaleDetail.CreatedUserId = _httpContextService.UserId;
        tblSaleDetail.CreatedDateTime = DateTime.Now;
        await _appDbContext.TblSaleDetails.AddAsync(tblSaleDetail);
        var result=await _appDbContext.SaveChangesAsync();

        return result <= 0
           ? Result<SaleDetailsModel>.Fail("Error Create")
           : Result<SaleDetailsModel>.Success("Success Create");
    }
}
