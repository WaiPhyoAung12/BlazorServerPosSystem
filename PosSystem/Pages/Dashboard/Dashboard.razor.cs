
using Microsoft.AspNetCore.Components;
using PosSystem.Constant;
using PosSystem.Models.Category;
using PosSystem.Services.Authentication;
using PosSystem.Services.Category;
using PosSystem.Services.Permission;

namespace PosSystem.Pages.Dashboard;

public partial class Dashboard
{
    private bool IsHavePermission;

    //[Inject]
    //public PermissionService permissionService=default!;
    [Inject]
    public PermissionService permissionService { get; set; }

    [Inject]
    public ICategoryService _categoryService { get; set; } = default!;

    [Inject]
    public NavigationManager _navigationManager { get; set; } = default!;

    public string ImagePath = "Images/";

    List<CategoryModel> CategoryList { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        IsHavePermission =await permissionService.CheckUserPermission(ConstantFunctionCode.CanAccessDashboard);
        if (!IsHavePermission)
            _navigationManager.NavigateTo("/403");

        var response=await _categoryService.GetCategoryList();

        if (response.IsError)
            return;

        CategoryList = response.Data!;
        
    }
   
}