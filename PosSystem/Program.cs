using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PosSystem.Data;
using PosSystem.Middlewares;
using MudBlazor;
using MudBlazor.Services;
using Microsoft.AspNetCore.Authentication;
using PosSystem.Services.Authentication;
using IAuthenticationService = PosSystem.Services.Authentication.IAuthenticationService;
using AuthenticationService = PosSystem.Services.Authentication.AuthenticationService;
using PosSystem.Domain.Login;
using Databases.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
using PosSystem.Models.Login;
using Microsoft.AspNetCore.Authentication.Cookies;
using PosSystem.Services.RoleFunction;
using PosSystem.Domain.RoleFunction;
using PosSystem.Services.Permission;
using Microsoft.Extensions.FileProviders;
using PosSystem.Services.Category;
using PosSystem.Domain.Category;
using Radzen;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMudServices();
builder.Services.AddRadzenComponents(); 
builder.Services.AddRazorPages();


builder.Services.AddServerSideBlazor();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();


builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IRoleFunctionService, RoleFunctionService>();
builder.Services.AddScoped<RoleFunctionRepo>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<LoginRepo>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<CategoryRepo>();
builder.Services.AddTransient<CookieMiddleware>();
builder.Services.AddSingleton<WeatherForecastService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<CookieMiddleware>();
app.UseRouting();

app.MapRazorPages();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();
