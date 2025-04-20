using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PosSystem.Models.Login;
using PosSystem.Services.Authentication;
using System.Security.Claims;
using IAuthenticationService = PosSystem.Services.Authentication.IAuthenticationService;

namespace PosSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]  
        public async Task<IActionResult> Login(LoginRequestModel loginRequestModel)
        {
           var response=await _authenticationService.Login(loginRequestModel);
            if (response.IsError)
                return View();

            var adminUser = response.Data;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, adminUser.RoleId.ToString()),
                new Claim(ClaimTypes.Name, adminUser.UserName),
                new Claim("UserId", adminUser.UserId),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            });
            return Redirect("/");
        }
    }
}
