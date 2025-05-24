using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Abstractions;
using MudBlazor;
using PosSystem.Models.Category;
using PosSystem.Models.Image;
using PosSystem.Models.Shared;
using PosSystem.Services.Category;
using PosSystem.Services.Dialog;
using Radzen;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PosSystem.Pages.Category;

public partial class CreateCategory
{
    public CategoryRequestModel Model { get; set; } = new();
    public string UploadFile { get; set; }

    public string ImagePath { get; set; }
    public string FileName { get; set; }
    [Inject]
    ICategoryService _categoryService { get; set; } = default!;

    [Inject]
    NavigationManager _navigationManager { get; set; } = default!;

    [Inject]
    IDialogServiceProvider dialogServiceProvider { get; set; } = default!;

    [Parameter]
    public bool IsEditForm { get; set; } = false;

    [Parameter]
    public string? Id { get; set; }

    [Inject]
    IOptions<ImageSetting> ImageSettingsOptions { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (Id is not null)
        {
            var response = await _categoryService.GetCategoryById(Convert.ToInt32(Id));
            if (response.IsSuccess)
            {
                Model.CategoryName = response.Data.CategoryName;
                Model.ImageName = response.Data.ImageName;
                Model.Id = response.Data.Id;
                Model.OldImageName = response.Data.ImageName;
                var settings = ImageSettingsOptions.Value;
                ImagePath = Path.Combine(settings.AppUrl, settings.ImageUrl,settings.CategoryImagesDirectory,Model.ImageName);
            }
            StateHasChanged();
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
        var response = new Result<CategoryModel>();

        if (IsEditForm)
            response = await _categoryService.UpdateCategory(Model);
        else
            response = await _categoryService.CategoryCreate(Model);

        string message = response.Message == null ? response.MessageList.FirstOrDefault() : response.Message;
        var dialogResult = await dialogServiceProvider.ShowConfirmDialogAsync(message, "Category");

        if(response.IsSuccess)
            _navigationManager.NavigateTo("/category/list", false);
        else
            return;

    }
}