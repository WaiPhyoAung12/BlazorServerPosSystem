using Microsoft.AspNetCore.Components;
using PosSystem.Models.Category;
using PosSystem.Services.Category;
using Radzen;
using System;

namespace PosSystem.Pages.Category;

public partial class CreateCategory
{
    public CategoryRequestModel Model { get; set; } = new();
    public string UploadFile { get; set; }
    public string FileName { get; set; }
    [Inject]
    ICategoryService _categoryService { get; set; } = default!;
    [Inject]
    NavigationManager _navigationManager { get; set; } = default!;
    private async Task OnChangeAsync(UploadChangeEventArgs args)
    {
        var file=args.Files.FirstOrDefault();
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
        var response=await _categoryService.CategoryCreate(Model);
        
    }
}