using PosSystem.Domain.Login;
using PosSystem.Models.Login;
using PosSystem.Models.Shared;

namespace PosSystem.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly LoginRepo _loginRepo;

    public AuthenticationService(LoginRepo loginRepo)
    {
        
        _loginRepo = loginRepo;
    }
    public async Task<Result<LoginResponseModel>> Login(LoginRequestModel loginRequest)
    {
        var result=await _loginRepo.Login(loginRequest);
        return result;
    }
}
