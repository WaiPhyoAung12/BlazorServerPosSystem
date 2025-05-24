
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using PosSystem.Constant;
using PosSystem.Models.Category;
using PosSystem.Models.Image;
using PosSystem.Services.Authentication;
using PosSystem.Services.Category;
using PosSystem.Services.Permission;

namespace PosSystem.Pages.Dashboard;

public partial class Dashboard
{
    private bool IsHavePermission;

    [Inject]
    public PermissionService permissionService { get; set; }

    [Inject]
    public ICategoryService _categoryService { get; set; } = default!;

    [Inject]
    public NavigationManager _navigationManager { get; set; } = default!;

    [Inject]
    IOptions<ImageSetting> ImageSettingsOptions { get; set; } = default!;

    public string ImageBaseUrl;

    List<CategoryModel> CategoryList { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        IsHavePermission =await permissionService.CheckUserPermission(ConstantFunctionCode.CanAccessDashboard);
        if (!IsHavePermission)
            _navigationManager.NavigateTo("/403");

        var response=await _categoryService.GetCategoryList();

        if (response.IsError)
            return;

        var settings = ImageSettingsOptions.Value;

        var imageUrl = Path.Combine(settings.AppUrl, settings.ImageUrl);

        ImageBaseUrl = string.Concat(imageUrl,"/",settings.CategoryImagesDirectory);

        CategoryList = response.Data!;
        
    }

    private void ClickCategory(CategoryModel categoryModel)
    {

    }
}