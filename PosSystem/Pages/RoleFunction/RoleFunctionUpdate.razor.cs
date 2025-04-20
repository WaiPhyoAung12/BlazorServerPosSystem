using Microsoft.AspNetCore.Components;
using PosSystem.Models.RoleFunction;
using PosSystem.Services.RoleFunction;

namespace PosSystem.Pages.RoleFunction;

public partial class RoleFunctionUpdate
{
    [Parameter]
    public int RoleId { get; set; }

    [Inject]
    public IRoleFunctionService _roleFunctionService { get; set; } = default!;

    public List<RoleFunctionResponseModel> Model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var response=await _roleFunctionService.GetRoleFunctionsByRoleId(RoleId);
        if (response.IsError)
            return;

        Model = response.Data!;
        StateHasChanged();
    }
}