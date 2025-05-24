
using MudBlazor;
using PosSystem.Pages.Shared;
using Radzen;

namespace PosSystem.Services.Dialog;

public class DialogServiceProvider : IDialogServiceProvider
{
    private readonly IDialogService _dialogService;

    public DialogServiceProvider(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public async void ShowDialogAsync(string message,string title)
    {
        MudBlazor.DialogOptions options = new MudBlazor.DialogOptions()
        {
            Position = MudBlazor.DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
        };
        var parameters = new DialogParameters<MudMessageDialog>
        {
            { x => x.DialogTitle, title },
            { x => x.DialogContent, message },
        };

        await _dialogService.ShowAsync<MudMessageDialog>("", parameters, options);
    }
    public async Task<DialogResult> ShowConfirmDialogAsync(string message,string title)
    {
        MudBlazor.DialogOptions options = new MudBlazor.DialogOptions()
        {
            Position = MudBlazor.DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
        };
        var parameters = new DialogParameters<MudMessageDialog>
        {
            { x => x.DialogTitle, title },
            { x => x.DialogContent, message },
        };
       var dialog= await _dialogService.ShowAsync<MudMessageDialog>("", parameters, options);
        return await dialog.Result;
    }

    public async Task<DialogResult> ShowConfirmDeleteDialogAsync(string title)
    {
        MudBlazor.DialogOptions options = new MudBlazor.DialogOptions()
        {
            Position = MudBlazor.DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
        };
        var parameters = new DialogParameters<MudDeleteConfirmDialog>
        {
            { x => x.DialogTitle, title },
        };
        var dialog = await _dialogService.ShowAsync<MudDeleteConfirmDialog>("", parameters, options);
        return await dialog.Result;
    }
}
