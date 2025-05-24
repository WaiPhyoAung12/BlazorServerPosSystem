using Microsoft.EntityFrameworkCore;
using Radzen.Blazor;
using Radzen;
using PosSystem.Models.Category;
using Microsoft.AspNetCore.Components;
using PosSystem.Services.Category;
using PosSystem.Services;
using Microsoft.Extensions.Options;
using PosSystem.Models.Image;
using PosSystem.Services.Dialog;
using PosSystem.Models.Product;

namespace PosSystem.Pages.Category;

public partial class ListCategory
{
    RadzenDataGrid<CategoryModel> dataGrid;

    IEnumerable<int> pageSizeOptions = new int[] { 10, 20, 30 };

    public CategoryListRequestModel categoryListRequestModel { get; set; } = new();

    List<CategoryModel> categoryList;

    int count;

    bool showPagerSummary = true;

    public string ImageBaseUrl;

    [Inject]
    ICategoryService categoryService { get; set; } = default!;

    [Inject]
    NavigationManager navigationManager { get; set; }= default!;

    [Inject]
    IOptions<ImageSetting> ImageSettingsOptions { get; set; } = default!;

    [Inject]
    IDialogServiceProvider dialogServiceProvider { get; set; } = default!;

    public string SearchValue { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var settings = ImageSettingsOptions.Value;

        ImageBaseUrl = Path.Combine(settings.AppUrl, settings.ImageUrl,settings.CategoryImagesDirectory);

        categoryList = new();
        
        //var response = await categoryService.GetCategoryList();
        //categoryList = response.Data;
        //StateHasChanged();
    }
    protected override void OnAfterRender(bool firstRender)
    {
        if(firstRender)
        {
            dataGrid.Reload();
        }
    }

    public async Task LoadDataAsync(LoadDataArgs args)
    {
        var pageSetting = args.GetPageSetting();
        categoryListRequestModel.PageSettingModel = pageSetting;
        categoryListRequestModel.PageSettingModel.SearchTitle = "";
         await GetCategoryList();
    }
    private async Task GetCategoryList()
    {
        var response = await categoryService.GetCategoryListPageSetting(categoryListRequestModel);
        if(response.IsSuccess)
        {
            categoryList = response.Data.CategoryModels;
            count = response.Data.PageSettingModel.TotalRowCount;
        }
       
    }

    public void EditCategory(CategoryModel categoryModel)
    {
        navigationManager.NavigateTo($"/category/edit/{categoryModel.Id}",false);
    }
    public async Task DeleteCategory(CategoryModel categoryModel)
    {
        var deleteDialogResult = await dialogServiceProvider.ShowConfirmDeleteDialogAsync("Category");
        if (deleteDialogResult.Canceled) return;
        
        var id =categoryModel.Id;
        var response=await categoryService.DeleteCategory(id);
        var dialogResult = await dialogServiceProvider.ShowConfirmDialogAsync(response.Message!, "Category Delete");
        if (dialogResult.Canceled || !dialogResult.Canceled)
        {
            if (response.IsSuccess)
                await GetCategoryList();
            else
                return;
        }
    }
    private async Task SearchButtonAction()
    {
        //if (string.IsNullOrEmpty(SearchValue))
        //    return;
        categoryListRequestModel.PageSettingModel.SearchValue = SearchValue;
        await GetCategoryList();
    }
    private void AddButtonAction()
    {
        navigationManager.NavigateTo("/category/list");
    }

}