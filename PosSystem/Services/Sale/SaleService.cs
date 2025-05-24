using PosSystem.Domain.Sale;
using PosSystem.Models.Sale;
using PosSystem.Models.Shared;

namespace PosSystem.Services.Sale;

public class SaleService : ISaleService
{
    private readonly SaleRepo _saleRepo;

    public SaleService(SaleRepo saleRepo)
    {
        _saleRepo = saleRepo;
    }
    public async Task<Result<SaleResponseModel>> PurchaseProduct(SaleRequestModel saleRequestModel)
    {
        var result=await _saleRepo.PurchaseProduct(saleRequestModel);
        return result;
    }
}
