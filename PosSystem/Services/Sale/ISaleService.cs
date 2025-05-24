using PosSystem.Models.Sale;
using PosSystem.Models.Shared;

namespace PosSystem.Services.Sale;

public interface ISaleService
{
    Task<Result<SaleResponseModel>> PurchaseProduct(SaleRequestModel saleRequestModel);
}
