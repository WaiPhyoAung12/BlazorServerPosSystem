using PosSystem.Models.PageSetting;

namespace PosSystem.Models.Category;

public class CategoryResponseModel
{
}
public class CategoryListResponseModel
{
    public List<CategoryModel> CategoryModels { get; set; }
    public PageSettingModel PageSettingModel { get; set; }
}
