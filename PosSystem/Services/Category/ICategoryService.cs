using PosSystem.Models.Category;
using PosSystem.Models.Product;
using PosSystem.Models.Shared;
using System.Threading.Tasks;

namespace PosSystem.Services.Category;

public interface ICategoryService
{
    Task<Result<List<CategoryModel>>> GetCategoryList();

    Task<Result<CategoryModel>>CategoryCreate(CategoryRequestModel categoryRequest);

    Task<Result<CategoryListResponseModel>> GetCategoryListPageSetting(CategoryListRequestModel categoryListRequest);

    Task<Result<CategoryModel>> GetCategoryById(int id);

    Task<Result<CategoryModel>> UpdateCategory(CategoryRequestModel categoryRequestModel);
    Task<Result<CategoryModel>> DeleteCategory(int id);
}
