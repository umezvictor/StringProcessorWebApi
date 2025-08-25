using Domain.Users;
using Shared.Responses;

namespace Application.Authentication
{
    public interface ITokenProvider
    {
        Task<GenerateAccessTokenResponse> GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken(string userId);
    }
}