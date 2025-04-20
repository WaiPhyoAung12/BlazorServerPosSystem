using Databases.AppDbContextModels;
using FluentValidation.Results;
using Mapster;
using Microsoft.EntityFrameworkCore;
using PosSystem.Models.Category;
using PosSystem.Models.Shared;
using System.Collections.Generic;
using System.Security.Claims;

namespace PosSystem.Domain.Category;

public class CategoryRepo
{
    private readonly AppDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CategoryRepo(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
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
        var result = CategoryRequestValidation(categoryRequest);
        if (!result.IsValid)
            return Result<CategoryModel>.FailValidation(result.Errors.Select(e => e.ErrorMessage).ToList());

        bool isDuplicate = CheckDuplicate(categoryRequest);
        if (isDuplicate)
            return Result<CategoryModel>.Fail("Data Already Exist");

        var imageName = GetImageName(categoryRequest.ImageName);

        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId))
            return Result<CategoryModel>.Fail("User Id is null");


        var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);
        using var stream = categoryRequest.ImageFile!.OpenReadStream();
        using var fs = new FileStream(wwwrootPath, FileMode.Create);
        await stream.CopyToAsync(fs);

        TblCategory category = new TblCategory()
        {
            CategoryName = categoryRequest.CategoryName,
            ImageName= imageName,
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
    private string GetImageName(string imageName)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        return $"{timestamp}-{imageName}";
    }
    private string GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity?.IsAuthenticated == true)
            return null;

        var userId = user.FindFirst(ClaimTypes.Name)?.Value;
        return string.IsNullOrEmpty(userId) ? null : userId;
    }
}
