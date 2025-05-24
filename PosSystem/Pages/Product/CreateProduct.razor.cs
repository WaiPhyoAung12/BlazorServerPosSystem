using Azure;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using PosSystem.Models.Category;
using PosSystem.Models.Image;
using PosSystem.Models.Product;
using PosSystem.Models.Shared;
using PosSystem.Services.Category;
using PosSystem.Services.Dialog;
using PosSystem.Services.Product;

namespace PosSystem.Pages.Product;

public partial class CreateProduct
{
    [Parameter]
    public string? ProductId { get; set; }

    [Inject]
    ICategoryService categoryService { get; set; } = default!;

    [Inject]
    NavigationManager _navigationManager { get; set; } = default!;

    [Inject]
    IDialogServiceProvider dialogServiceProvider { get; set; } = default!;

    [Inject]
    IProductService productService { get; set; } = default!;

    [Inject]
    IOptions<ImageSetting> ImageSettingsOptions { get; set; } = default!;


    public ProductRequestModel Model { get; set; } = new();
    public string UploadFile { get; set; }
    public string FileName { get; set; }
    public string CategoryName { get; set; }
    public string ImagePath { get; set; }

    public List<CategoryModel> CategoryList { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        if(ProductId is not null)
        {
            var getProductResponse=await productService.GetProductById(Convert.ToInt32(ProductId));
            if (getProductResponse.IsSuccess)
            {
                Model = getProductResponse.Data.Adapt<ProductRequestModel>();
                var settings = ImageSettingsOptions.Value;
                ImagePath = Path.Combine(settings.AppUrl, settings.ImageUrl, settings.ProductImagesDirectory, Model.ImagePath);
            }
        }

        var response = await categoryService.GetCategoryList();

        if (response.IsSuccess)
            CategoryList = response.Data!;
        else
        {
            string message = response.Message == null ? response.MessageList.FirstOrDefault() : response.Message;
            var dialogResult = await dialogServiceProvider.ShowConfirmDialogAsync(message, "Category");
        }
    }

    private async Task OnChangeAsync(IBrowserFile file)
    {
        if (file != null)
        {
            using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // 10MB limit
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var bytes = ms.ToArray();
            var base64 = Convert.ToBase64String(bytes);
            var contentType = file.ContentType;
            UploadFile = $"data:{contentType};base64,{base64}";
            FileName = file.Name;
            Model.ImageFile = file;
            Model.ImageName = file.Name;
        }
        StateHasChanged();
    }
    private async Task SubmitCategory()
    {
        var response = await productService.CreateProduct(Model);
        string message = response.Message == null ? response.MessageList.FirstOrDefault() : response.Message;
        var dialogResult = await dialogServiceProvider.ShowConfirmDialogAsync(message, "Category");

        if (response.IsSuccess)
            _navigationManager.NavigateTo("/product/list", false);
        else
            return;
    }
}