using Databases.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
using PosSystem.Models.Login;
using PosSystem.Models.Shared;
using PosSystem.Services.PasswordHasher;

namespace PosSystem.Domain.Login;

public class LoginRepo
{
    private readonly AppDbContext _appDbContext;

    public LoginRepo(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<Result<LoginResponseModel>>Login(LoginRequestModel loginRequestModel)
    {
        bool isValidate=LoginRequestValidation(loginRequestModel);
        if (!isValidate)
            return Result<LoginResponseModel>.Fail("User name or password is invalid");

        var adminUser=await _appDbContext.TblUsers.FirstOrDefaultAsync(x=>x.Name==loginRequestModel.UserName);

        if(adminUser is null)
            return Result<LoginResponseModel>.Fail("Invalid user name");

        bool isValidPassword = PasswordHasherService
            .PasswordValidation(adminUser.Password, loginRequestModel.Password, adminUser.Salt);

        if(!isValidPassword)
            return Result<LoginResponseModel>.Fail("Invalid password");

        LoginResponseModel responseModel = new LoginResponseModel()
        {
            UserId = adminUser.Id,
            UserName = adminUser.Name,
            RoleId = adminUser.RoleId
        };

        return Result<LoginResponseModel>.Success("Success",responseModel);
    }
    private bool LoginRequestValidation(LoginRequestModel loginRequestModel)
    {
        LoginRequestValidator validator = new();
        var result=validator.Validate(loginRequestModel);
        return result.IsValid;
    }

    
}
