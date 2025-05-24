using PosSystem.Domain.Category;
using PosSystem.Models.Category;
using PosSystem.Models.Shared;

namespace PosSystem.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly CategoryRepo _categoryRepo;

    public CategoryService(CategoryRepo categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<Result<CategoryModel>> CategoryCreate(CategoryRequestModel categoryRequest)
    {
        var result=await _categoryRepo.CreateCategory(categoryRequest);
        return result;
    }

    public async Task<Result<CategoryModel>> DeleteCategory(int id)
    {
        var result=await _categoryRepo.DeleteCategory(id);
        return result;
    }

    public async Task<Result<CategoryModel>> GetCategoryById(int id)
    {
        var result=await _categoryRepo.GetCategoryById(id);
        return result;
    }

    public async Task<Result<List<CategoryModel>>> GetCategoryList()
    {
        var result=await _categoryRepo.GetCategoryList();
        return result;
    }

    public async Task<Result<CategoryListResponseModel>> GetCategoryListPageSetting(CategoryListRequestModel categoryListRequest)
    {
        var result=await _categoryRepo.GetCategoryListPageSetting(categoryListRequest);
        return result;
    }

    public async Task<Result<CategoryModel>> UpdateCategory(CategoryRequestModel categoryRequestModel)
    {
        var result=await _categoryRepo.UpdateCategory(categoryRequestModel);
        return result;
    }
}
