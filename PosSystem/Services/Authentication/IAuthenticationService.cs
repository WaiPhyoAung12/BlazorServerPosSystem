using PosSystem.Models.Login;
using PosSystem.Models.Shared;

namespace PosSystem.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<Result<LoginResponseModel>>Login(LoginRequestModel loginRequest);
    }
}
