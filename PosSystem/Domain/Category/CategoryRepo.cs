using Databases.AppDbContextModels;
using FluentValidation.Results;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PosSystem.Models.Category;
using PosSystem.Models.Image;
using PosSystem.Models.PageSetting;
using PosSystem.Models.Shared;
using PosSystem.Services;
using PosSystem.Services.Shared;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace PosSystem.Domain.Category;

public class CategoryRepo
{
    private readonly AppDbContext _appDbContext;

    private readonly HttpContextService _httpContextService;
    private readonly ImageSetting _imageSetting;

    public CategoryRepo(AppDbContext appDbContext, HttpContextService httpContextService, IOptionsMonitor<ImageSetting> imageSetting)
    {
        _appDbContext = appDbContext;
        _httpContextService = httpContextService;
        _imageSetting = imageSetting.CurrentValue;
    }

    public async Task<Result<List<CategoryModel>>> GetCategoryList()
    {
        List<CategoryModel> categoryModels = new List<CategoryModel>();
        var response = await _appDbContext.TblCategories.Where(x => x.DeleteFlag == false).ToListAsync();
        if (response.Count <= 0)
            return Result<List<CategoryModel>>.Fail("Data Not Found");

        categoryModels = response.Adapt<List<CategoryModel>>();
        return Result<List<CategoryModel>>.Success("Success", categoryModels);

    }

    public async Task<Result<CategoryModel>> CreateCategory(CategoryRequestModel categoryRequest)
    {
        try
        {
            var result = CategoryRequestValidation(categoryRequest);
            if (!result.IsValid)
                return Result<CategoryModel>.FailValidation(result.Errors.Select(e => e.ErrorMessage).ToList());

            bool isDuplicate = CheckDuplicate(categoryRequest);
            if (isDuplicate)
                return Result<CategoryModel>.Fail("Data Already Exist");

            string imageName = categoryRequest.ImageName.GetImageName();

            var userId = _httpContextService.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<CategoryModel>.Fail("User Id is null");

            bool saveImageResult = await SaveCategoryImages(imageName, categoryRequest);

            if (!saveImageResult)
                return Result<CategoryModel>.Fail("Save Image Error");

            TblCategory category = new TblCategory()
            {
                CategoryName = categoryRequest.CategoryName,
                ImageName = imageName,
                CreatedDateTime = DateTime.Now,
                CreatedUserId = userId,
                DeleteFlag = false,
            };

            await _appDbContext.AddAsync(category);
            var response = await _appDbContext.SaveChangesAsync();
            return response <= 0 ?

                             Result<CategoryModel>.Fail("Error Occur in Creation")
                :
                             Result<CategoryModel>.Success("Success Create");
        }
        catch (Exception e)
        {

            throw;
        }

    }

    private ValidationResult CategoryRequestValidation(CategoryRequestModel categoryRequest)
    {
        CategoryRequestValidator validator = new();
        var result = validator.Validate(categoryRequest);
        return result;
    }

    private bool CheckDuplicate(CategoryRequestModel categoryRequest)
    {
        bool IsDuplicate = false;
        if (categoryRequest.Id != 0)
        {
            IsDuplicate = _appDbContext.TblCategories.Any(x =>
            x.DeleteFlag == false &&
            x.Id != categoryRequest.Id &&
            x.CategoryName.Trim().ToLower() == categoryRequest.CategoryName.Trim().ToLower()
            );
        }
        else
        {
            IsDuplicate = _appDbContext.TblCategories.Any(x =>
            x.DeleteFlag == false &&
            x.CategoryName.Trim().ToLower() == categoryRequest.CategoryName.Trim().ToLower()
            );
        }
        return IsDuplicate;
    }

    public async Task<Result<CategoryListResponseModel>> GetCategoryListPageSetting(CategoryListRequestModel categoryListRequest)
    {
        var pageSetting = categoryListRequest.PageSettingModel;

        if (pageSetting is null || pageSetting.IsNotValid)
            return Result<CategoryListResponseModel>.Fail("Invalid Page Setting");

        var query = from ca in _appDbContext.TblCategories
                    where ca.DeleteFlag == false
                    select new CategoryModel
                    {
                        Id = ca.Id,
                        CategoryName = ca.CategoryName,
                        CreatedDateTime = ca.CreatedDateTime.Date,
                        CreatedUserId = ca.CreatedUserId,
                        UpdatedDateTime = ca.UpdatedDateTime,
                        UpdatedUserId = ca.UpdatedUserId,
                        ImageName = ca.ImageName,
                    };

        #region Searching

        string? searchString = pageSetting!.SearchValue.Trim().ToLower();
        string? searchTitle = pageSetting.SearchTitle == null ? null : pageSetting!.SearchTitle.Trim().ToLower();

        if (!searchString!.IsNullOrEmpty())
        {
            query = query.Where(x => x.CategoryName.Contains(searchString!)
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
        CategoryListResponseModel categoryListResponseModel = new();
        categoryListResponseModel.CategoryModels = datalist;
        categoryListResponseModel.PageSettingModel = pageSetting.GetPageSettingResponse(totalCount);

        return Result<CategoryListResponseModel>.Success("Success", categoryListResponseModel);
    }

    public async Task<Result<CategoryModel>> GetCategoryById(int id)
    {
        if (id is 0)
            return Result<CategoryModel>.Fail("Bad Request");

        var response = await _appDbContext.TblCategories.FirstOrDefaultAsync(x => x.Id == id && x.DeleteFlag == false);

        if (response is null)
            return Result<CategoryModel>.Fail("Data Not Found");

        CategoryModel categoryModel = new();
        categoryModel = response.Adapt<CategoryModel>();
        return Result<CategoryModel>.Success("Success", categoryModel);
    }

    public async Task<Result<CategoryModel>> DeleteCategory(int id)
    {
        if (id is 0)
            return Result<CategoryModel>.Fail("Bad Request");

        var result = await _appDbContext.TblCategories
            .Where(x => x.DeleteFlag == false && x.Id == id)
            .ExecuteUpdateAsync(x => x
            .SetProperty(c => c.DeleteFlag, true));

        if (result <= 0)
            return Result<CategoryModel>.Fail("Error in update");

        var response = await _appDbContext.TblProducts
            .Where(x => x.DeleteFlag == false && x.CategoryId == id)
            .ExecuteUpdateAsync(x => x
            .SetProperty(p => p.DeleteFlag, true));

        if (response <= 0)
            return Result<CategoryModel>.Fail("Error in update");

        return Result<CategoryModel>.Success("Success delete");
    }


    public async Task<Result<CategoryModel>> UpdateCategory(CategoryRequestModel categoryRequestModel)
    {
        var result = CategoryRequestValidation(categoryRequestModel);
        if (!result.IsValid)
            return Result<CategoryModel>.FailValidation(result.Errors.Select(e => e.ErrorMessage).ToList());

        bool isDuplicate = CheckDuplicate(categoryRequestModel);
        if (isDuplicate)
            return Result<CategoryModel>.Fail("Data Already Exist");

        var imageName = categoryRequestModel.OldImageName;

        if(categoryRequestModel.ImageFile is not null)
        {
            imageName = categoryRequestModel.ImageName.GetImageName();
            bool successSave = await SaveCategoryImages(imageName, categoryRequestModel);
            if (!successSave)
                return Result<CategoryModel>.Fail("Image Save error");
        }      

        var userId = _httpContextService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Result<CategoryModel>.Fail("User Id is null");

        var response = _appDbContext.TblCategories
            .Where(x => x.Id == categoryRequestModel.Id && x.DeleteFlag == false)
            .ExecuteUpdate(x => x
            .SetProperty(a => a.CategoryName, categoryRequestModel.CategoryName)
            .SetProperty(a => a.ImageName, imageName)
            .SetProperty(a => a.UpdatedDateTime, DateTime.Now)
            .SetProperty(a => a.UpdatedUserId, userId)
            );

        if (response <= 0)
            return Result<CategoryModel>.Fail("Fail update");

        return Result<CategoryModel>.Success("Success update");
    }

    private async Task<bool> SaveCategoryImages(string imageName, CategoryRequestModel categoryRequestModel)
    {
        try
        {
           
            if (categoryRequestModel.OldImageName is not null)
            {
                var imagePath = Path.Combine(_imageSetting.ImageBaseDirectory, categoryRequestModel.OldImageName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }

            }
            var folderPath = @"C:\Exercises\PosSystemImages\Categories";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var savePath = Path.Combine(folderPath, imageName);

            using var stream = categoryRequestModel.ImageFile!.OpenReadStream(maxAllowedSize: 1024 * 1024 * 5); // 5 MB);
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
