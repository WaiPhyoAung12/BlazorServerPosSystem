using Microsoft.AspNetCore.Components;
using PosSystem.Models.Category;
using PosSystem.Services.Category;
using Radzen.Blazor;
using Radzen;
using PosSystem.Models.Product;
using PosSystem.Services;
using PosSystem.Services.Product;
using Microsoft.Extensions.Options;
using PosSystem.Models.Image;
using PosSystem.Services.Dialog;

namespace PosSystem.Pages.Product;

public partial class ProductList
{
    [Parameter]
    public string CategoryId { get; set; }

    RadzenDataGrid<ProductModel> dataGrid;

    IEnumerable<int> pageSizeOptions = new int[] { 10, 20, 30 };

    public ProductListRequestModel productListRequestModel { get; set; } = new();

    List<ProductModel> productList;

    int count;

    bool showPagerSummary = true;

    [Inject]
    IProductService productService { get; set; } = default!;

    [Inject]
    NavigationManager navigationManager { get; set; } = default!;

    [Inject]
    IOptions<ImageSetting> ImageSettingsOptions { get; set; } = default!;

    [Inject]
    IDialogServiceProvider dialogServiceProvider { get; set; } = default!;

    public string ImageBaseUrl;
    public string SearchValue { get; set; }
    protected override async Task OnInitializedAsync()
    {
        var settings = ImageSettingsOptions.Value;
        ImageBaseUrl = Path.Combine(settings.AppUrl, settings.ImageUrl, settings.ProductImagesDirectory);
        productList = new();

        //var response = await categoryService.GetCategoryList();
        //categoryList = response.Data;
        //StateHasChanged();
    }
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            dataGrid.Reload();
        }
    }

    public async Task LoadDataAsync(LoadDataArgs args)
    {
        var pageSetting = args.GetPageSetting();
        productListRequestModel.PageSetting = pageSetting;
        productListRequestModel.PageSetting.SearchTitle = "";
        if (CategoryId is not null)
            productListRequestModel.CategoryId = Convert.ToInt32(CategoryId);
        await GetProductList();
    }
    private async Task GetProductList()
    {
        var response = await productService.GetProductListPageSetting(productListRequestModel);
        if (response.IsSuccess)
        {
            productList = response.Data.ProductModels;
            count = response.Data.PageSetting.TotalRowCount;
        }

    }

    public void EditCategory(ProductModel productModel)
    {
        navigationManager.NavigateTo($"/product/edit/{productModel.Id}", false);
        StateHasChanged();
    }

    public async Task DeleteProduct(ProductModel productModel)
    {
        var deleteDialogResult = await dialogServiceProvider.ShowConfirmDeleteDialogAsync("Product");
        if (deleteDialogResult.Canceled) return;

        var id = productModel.Id;
        var response = await productService.DeleteProduct(id);
        var dialogResult = await dialogServiceProvider.ShowConfirmDialogAsync(response.Message!, "Product Delete");
        if (dialogResult.Canceled || !dialogResult.Canceled)
        {
            if (response.IsSuccess)
                await GetProductList();
            else
                return;
        }
    }
    private void AddButtonAction()
    {
        navigationManager.NavigateTo("/product/create");
    }

    private async Task SearchButtonAction()
    {
        //if (string.IsNullOrEmpty(SearchValue))
        //    return;
        productListRequestModel.PageSetting.SearchValue = SearchValue;
        await GetProductList();
    }
}