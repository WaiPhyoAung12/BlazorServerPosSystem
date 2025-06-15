using Databases.AppDbContextModels;
using FluentValidation.Results;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PosSystem.Models.Category;
using PosSystem.Models.Image;
using PosSystem.Models.Product;
using PosSystem.Models.Shared;
using PosSystem.Services;
using PosSystem.Services.Shared;

namespace PosSystem.Domain.Product;

public class ProductRepo
{
    private readonly AppDbContext _appDbContext;
    private readonly HttpContextService _httpContextService;
    private readonly ImageSetting _imageSetting;

    public ProductRepo(AppDbContext appDbContext, HttpContextService httpContextService, IOptionsMonitor<ImageSetting> imageSetting)
    {
        _appDbContext = appDbContext;
        _httpContextService = httpContextService;
        _imageSetting = imageSetting.CurrentValue;
    }

    public async Task<Result<ProductModel>> CreateProduct(ProductRequestModel productRequest)
    {
        try
        {
            var result = ProductRequestValidation(productRequest);
            if (!result.IsValid)
                return Result<ProductModel>.FailValidation(result.Errors.Select(e => e.ErrorMessage).ToList());

            bool isDuplicate = CheckDuplicate(productRequest);
            if (isDuplicate)
                return Result<ProductModel>.Fail("Data Already Exist");

            var imageName = productRequest.ImageName.GetImageName();

            var userId = _httpContextService.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<ProductModel>.Fail("User Id is null");

            var folderPath = @"C:\Exercises\PosSystemImages\Products";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var savePath = Path.Combine(folderPath, imageName);

            //var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);
            using var stream = productRequest.ImageFile!.OpenReadStream(maxAllowedSize: 1024 * 1024 * 5); // 5 MB);
            using var fs = new FileStream(savePath, FileMode.Create);
            await stream.CopyToAsync(fs);

            TblProduct product = new TblProduct()
            {
                ProductName = productRequest.ProductName,
                ImagePath = imageName,
                CategoryId = productRequest.CategoryId,
                StockQuantity = productRequest.StockQuantity,
                Price = productRequest.Price,
                BarCode = 1,
                CreatedDateTime = DateTime.Now,
                CreatedUserId = userId,
                DeleteFlag = false,
            };

            await _appDbContext.TblProducts.AddAsync(product);
            var response = await _appDbContext.SaveChangesAsync();
            return response <= 0 ?

                             Result<ProductModel>.Fail("Error Occur in Creation")
                :
                             Result<ProductModel>.Success("Success Create");
        }
        catch (Exception e)
        {

            throw;
        }

    }

    private ValidationResult ProductRequestValidation(ProductRequestModel productRequest)
    {
        ProductRequestValidator validator = new();
        var result = validator.Validate(productRequest);
        return result;
    }

    public async Task<Result<ProductListResponseModel>> GetProductListPageSetting(ProductListRequestModel productListRequest)
    {
        var pageSetting = productListRequest.PageSetting;

        if (pageSetting is null || pageSetting.IsNotValid)
            return Result<ProductListResponseModel>.Fail("Invalid Page Setting");

        IQueryable<ProductModel> query;

        if (productListRequest.CategoryId is not null)
        {
            query = from ca in _appDbContext.TblCategories
                    join p in _appDbContext.TblProducts
                    on ca.Id equals p.CategoryId
                    where p.DeleteFlag == false
                    && ca.DeleteFlag == false
                    && ca.Id == productListRequest.CategoryId
                    select new ProductModel
                    {
                        Id = p.Id,
                        ProductName = p.ProductName,
                        CreatedDateTime = p.CreatedDateTime.Date,
                        ImagePath = p.ImagePath,
                        CategoryId = p.CategoryId,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        CategoryName = ca.CategoryName,
                    };
        }
        else
        {
            query = from p in _appDbContext.TblProducts
                    join ca in _appDbContext.TblCategories
                    on p.CategoryId equals ca.Id
                    where p.DeleteFlag == false && ca.DeleteFlag == false
                    select new ProductModel
                    {
                        Id = p.Id,
                        ProductName = p.ProductName,
                        CreatedDateTime = p.CreatedDateTime.Date,
                        ImagePath = p.ImagePath,
                        CategoryId = p.CategoryId,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        CategoryName = ca.CategoryName,
                    };
        }



        #region Searching

        string? searchString = pageSetting!.SearchValue.Trim().ToLower();
        string? searchTitle = pageSetting.SearchTitle == null ? null : pageSetting!.SearchTitle.Trim().ToLower();

        if (!searchString!.IsNullOrEmpty())
        {
            query = query.Where(x => x.ProductName.Contains(searchString!)
                                    || x.Price.Equals(searchString!.ToInt())
                                    || x.StockQuantity.Equals(searchString!.ToInt())
                                    || x.CategoryName.Contains(searchString!)
                                     || x.CreatedDateTime.ToString().Contains(searchString!)
                                     );
        }

        var totalCount = query.ToList().Count();

        #endregion

        #region Sorting

        var sorting = pageSetting.Sorting;
        var noDistinctQuery = query;
        query = sorting is not null
            ? query.Sort(sorting.Key, sorting.Order)
            : query.OrderByDescending(x => x.Id);

        #endregion

        #region Pagination

        query = query.Skip(pageSetting.SkipCount).Take(pageSetting.PageSize);
        var datalist = query.ToList();

        #endregion
        ProductListResponseModel productListResponseModel = new();
        productListResponseModel.ProductModels = datalist;
        productListResponseModel.PageSetting = pageSetting.GetPageSettingResponse(totalCount);

        return Result<ProductListResponseModel>.Success("Success", productListResponseModel);
    }

    private bool CheckDuplicate(ProductRequestModel productRequestModel)
    {
        bool IsDuplicate = false;
        if (productRequestModel.Id != 0)
        {
            IsDuplicate = _appDbContext.TblProducts.Any(x =>
            x.DeleteFlag == false &&
            x.Id != productRequestModel.Id &&
            x.ProductName.Trim().ToLower() == productRequestModel.ProductName.Trim().ToLower()
            );
        }
        else
        {
            IsDuplicate = _appDbContext.TblProducts.Any(x =>
            x.DeleteFlag == false &&
            x.ProductName.Trim().ToLower() == productRequestModel.ProductName.Trim().ToLower()
            );
        }
        return IsDuplicate;
    }

    public async Task<Result<ProductModel>> UpdateProduct(ProductRequestModel productRequestModel)
    {
        var result = ProductRequestValidation(productRequestModel);
        if (!result.IsValid)
            return Result<ProductModel>.FailValidation(result.Errors.Select(e => e.ErrorMessage).ToList());

        bool isDuplicate = CheckDuplicate(productRequestModel);
        if (isDuplicate)
            return Result<ProductModel>.Fail("Data Already Exist");

        var imageName = productRequestModel.OldImageName;

        if (productRequestModel.ImageFile is not null)
        {
            imageName = productRequestModel.ImageName.GetImageName();
            bool successSave = await SaveProductImages(imageName, productRequestModel);
            if (!successSave)
                return Result<ProductModel>.Fail("Image Save error");
        }

        //var imageName = productRequestModel.ImageName.GetImageName();

        //bool successSave = await SaveProductImages(imageName, productRequestModel);
        //if (!successSave)
        //    return Result<ProductModel>.Fail("Image Save error");

        var userId = _httpContextService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Result<ProductModel>.Fail("User Id is null");

        var response = _appDbContext.TblProducts
            .Where(x => x.Id == productRequestModel.Id && x.DeleteFlag == false)
            .ExecuteUpdate(x => x
            .SetProperty(a => a.ProductName, productRequestModel.ProductName)
            .SetProperty(a => a.ImagePath, imageName)
            .SetProperty(a => a.CategoryId, productRequestModel.CategoryId)
            .SetProperty(a => a.StockQuantity, productRequestModel.StockQuantity)
            .SetProperty(a => a.Price, productRequestModel.Price)
            .SetProperty(a => a.UpdatedDateTime, DateTime.Now)
            .SetProperty(a => a.UpdatedUserId, userId)
            );

        if (response <= 0)
            return Result<ProductModel>.Fail("Fail update");

        return Result<ProductModel>.Success("Success update");
    }

    public async Task<Result<ProductModel>> GetProductById(int id)
    {
        if (id is 0)
            return Result<ProductModel>.Fail("Bad Request");

        var response = await _appDbContext.TblProducts.FirstOrDefaultAsync(x => x.Id == id && x.DeleteFlag == false);

        if (response is null)
            return Result<ProductModel>.Fail("Data Not Found");

        ProductModel productModel = new();
        productModel = response.Adapt<ProductModel>();
        return Result<ProductModel>.Success("Success", productModel);
    }

    public async Task<Result<ProductModel>>GetProductByBarCode(int barCode)
    {
        if (barCode is 0)
            return Result<ProductModel>.Fail("Bad Request");

        var response = await _appDbContext.TblProducts.FirstOrDefaultAsync(x => x.BarCode == barCode && x.DeleteFlag == false);

        if (response is null)
            return Result<ProductModel>.Fail("Data Not Found");

        ProductModel productModel = new();
        productModel = response.Adapt<ProductModel>();
        return Result<ProductModel>.Success("Success", productModel);
    }

    public async Task<Result<ProductModel>> DeleteProduct(int id)
    {
        var response = await _appDbContext.TblProducts
            .Where(x => x.DeleteFlag == false && x.Id==id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.DeleteFlag, true));
        if (response <= 0)
            return Result<ProductModel>.Fail("Fail in update");

        return Result<ProductModel>.Success("Success delete");
    }

    private async Task<bool> SaveProductImages(string imageName, ProductRequestModel productRequestModel)
    {
        try
        {
            if (productRequestModel.OldImageName is not null)
            {
                var imagePath = Path.Combine(_imageSetting.ImageBaseDirectory, productRequestModel.OldImageName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }

            }
            var folderPath = @"C:\Exercises\PosSystemImages\Products";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var savePath = Path.Combine(folderPath, imageName);

            using var stream = productRequestModel.ImageFile!.OpenReadStream(maxAllowedSize: 1024 * 1024 * 5); // 5 MB);
            using var fs = new FileStream(savePath, FileMode.Create);
            await stream.CopyToAsync(fs);
            return true;
        }
        catch (Exception)
        {

            return false;
        }
    }

}
