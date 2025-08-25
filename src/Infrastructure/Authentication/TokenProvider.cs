using Application.Authentication;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Responses;
using Shared.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Authentication
{
    public class TokenProvider(UserManager<User> userManager, IOptions<JwtSettings> jwtSettings) : ITokenProvider
    {
        public async Task<GenerateAccessTokenResponse> GenerateAccessToken(User user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Role, roles[0]),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),

                };
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.SigninKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            int tokenLifeTime = 3600; //1hour
            DateTime expiresOnUtc = DateTime.UtcNow.AddSeconds(tokenLifeTime);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtSettings.Value.Issuer,
                audience: jwtSettings.Value.Audience,
                claims: claims,
                expires: expiresOnUtc,
                signingCredentials: signingCredentials);
            return new GenerateAccessTokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken),
                ExpiresIn = tokenLifeTime
            };
        }


        public RefreshToken GenerateRefreshToken(string userId)
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = userId
            };

        }


    }
}
