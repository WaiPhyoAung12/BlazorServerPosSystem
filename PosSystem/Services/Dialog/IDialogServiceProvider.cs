using MudBlazor;

namespace PosSystem.Services.Dialog;

public interface IDialogServiceProvider
{
    void ShowDialogAsync(string message, string title);
    Task<DialogResult> ShowConfirmDialogAsync(string message, string title);
    Task<DialogResult> ShowConfirmDeleteDialogAsync(string title);
}
