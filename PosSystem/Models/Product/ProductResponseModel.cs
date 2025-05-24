using PosSystem.Models.PageSetting;

namespace PosSystem.Models.Product;

public class ProductResponseModel
{
}
public class ProductListResponseModel
{
    public List<ProductModel> ProductModels { get; set; }
    public PageSettingModel PageSetting { get; set; }
}
