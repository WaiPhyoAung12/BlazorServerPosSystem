
using PosSystem.Models.Product;
using PosSystem.Models.Shared;

namespace PosSystem.Services.Product;

public interface IProductService
{
    Task<Result<ProductModel>> CreateProduct(ProductRequestModel productRequest);
    Task<Result<ProductListResponseModel>> GetProductListPageSetting(ProductListRequestModel productListRequest);
    Task<Result<ProductModel>> UpdateProduct(ProductRequestModel productRequestModel);
    Task<Result<ProductModel>> GetProductById(int id);
    Task<Result<ProductModel>> DeleteProduct(int id);
    Task<Result<ProductModel>> GetProductByBarCode(int barCode);
}
