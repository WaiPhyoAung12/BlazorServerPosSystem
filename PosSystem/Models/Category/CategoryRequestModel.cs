using Microsoft.AspNetCore.Components.Forms;
using PosSystem.Models.PageSetting;

namespace PosSystem.Models.Category;

public class CategoryRequestModel
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public string ImageName { get; set; }

    public string? OldImageName { get; set; }
    public IBrowserFile? ImageFile { get; set; }
}
public class CategoryListRequestModel
{
    public PageSettingModel PageSettingModel { get; set; }
}
