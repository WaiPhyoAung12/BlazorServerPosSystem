using PosSystem.Models.Category;
using PosSystem.Models.Shared;

namespace PosSystem.Services.Category;

public interface ICategoryService
{
    Task<Result<List<CategoryModel>>> GetCategoryList();
    Task<Result<CategoryModel>>CategoryCreate(CategoryRequestModel categoryRequest);
}
