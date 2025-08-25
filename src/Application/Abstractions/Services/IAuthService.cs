using Shared;
using Shared.Requests;
using Shared.Responses;

namespace Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<Result<AuthenticationResponse>> Login(AuthenticationRequest request);
        Task<Result<RefreshTokenResponse>> GenerateRefreshTokenAsync(RefreshTokenRequest request);
    }
}