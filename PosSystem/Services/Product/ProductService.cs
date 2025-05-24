using PosSystem.Domain.Product;
using PosSystem.Models.Product;
using PosSystem.Models.Shared;

namespace PosSystem.Services.Product;

public class ProductService : IProductService
{
    private readonly ProductRepo _productRepo;

    public ProductService(ProductRepo productRepo)
    {
        _productRepo = productRepo;
    }
    public async Task<Result<ProductModel>> CreateProduct(ProductRequestModel productRequest)
    {
        var result=await _productRepo.CreateProduct(productRequest);
        return result;
    }

    public async Task<Result<ProductModel>> DeleteProduct(int id)
    {
        var result=await _productRepo.DeleteProduct(id);
        return result;
    }

    public async Task<Result<ProductModel>> GetProductByBarCode(int barCode)
    {
        var result=await _productRepo.GetProductByBarCode(barCode);
        return result;
    }

    public async Task<Result<ProductModel>> GetProductById(int id)
    {
        var result=await _productRepo.GetProductById(id);
        return result;
    }

    public async Task<Result<ProductListResponseModel>> GetProductListPageSetting(ProductListRequestModel productListRequest)
    {
        var result=await _productRepo.GetProductListPageSetting(productListRequest);
        return result;
    }

    public async Task<Result<ProductModel>> UpdateProduct(ProductRequestModel productRequestModel)
    {
        var result=await _productRepo.UpdateProduct(productRequestModel);
        return result;
    }
}
