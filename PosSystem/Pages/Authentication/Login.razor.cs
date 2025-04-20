using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using PosSystem.Models.Login;
using PosSystem.Services.Authentication;
using System.Security.Claims;
using IAuthenticationService = PosSystem.Services.Authentication.IAuthenticationService;

namespace PosSystem.Pages.Authentication;

public partial class Login
{
    [Inject]
    public IAuthenticationService _authenticationService { get; set; } = default!;

    [Inject]
    public IHttpContextAccessor _httpcontextAccessor { get; set; } = default!;

    [Inject]
    public IAuthenticationService AuthenticationService { get; set; }=default!;

    [Inject]
    public NavigationManager _navigationManager { get; set; } = default!;

    public LoginRequestModel Model { get; set; } = new();
    public string Message { get;set; }

    private async Task SubmitLogin()
    {
        var response = await _authenticationService.Login(Model);
        if (response.IsError)
        {
            Message = response.Message!;
            StateHasChanged();
            return;
        }
            
        var adminUser = response.Data;
        var claims=new List<Claim>();
        {
            new Claim(ClaimTypes.Role, adminUser.RoleId.ToString());
            new Claim(ClaimTypes.Name, adminUser.UserName);
            new Claim("UserId", adminUser.UserId);
        }
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        
        _navigationManager.NavigateTo("/", false);
    }
}