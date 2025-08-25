using Application.Abstractions.Services;
using Application.Authentication;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Shared;
using Shared.Requests;
using Shared.Responses;

namespace Infrastructure.Services
{
    public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager,
            ApplicationDbContext todoContext, ITokenProvider tokenProvider, IMemoryCache memoryCache) : IAuthService
    {

        public async Task<Result<AuthenticationResponse>> Login(AuthenticationRequest request)
        {

            AuthenticationResponse response = new AuthenticationResponse();
            string cacheKey = $"user_email_{request.Email}";

            var user = await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
                var userFromDb = await userManager.FindByEmailAsync(request.Email);
                return userFromDb;
            });

            if (user is null)
                return Result.Failure<AuthenticationResponse>(UserErrors.InvalidCredentials);


            var result = await signInManager.PasswordSignInAsync(user.Email!, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Result.Failure<AuthenticationResponse>(UserErrors.InvalidCredentials);

            if (!user.EmailConfirmed)
                return Result.Failure<AuthenticationResponse>(UserErrors.EmailNotVerified);

            //create access token
            GenerateAccessTokenResponse accessTokenResponse = await tokenProvider.GenerateAccessToken(user);
            response.AccessToken = accessTokenResponse.AccessToken;
            response.Email = user.Email!;
            response.UserName = user.UserName!;
            response.UserId = user.Id;
            response.ExpiresIn = accessTokenResponse.ExpiresIn;

            //create refresh token
            var refreshToken = tokenProvider.GenerateRefreshToken(user.Id);
            todoContext.RefreshTokens.Add(refreshToken);
            await todoContext.SaveChangesAsync();

            response.RefreshToken = refreshToken.Token;

            return response;
        }


        public async Task<Result<RefreshTokenResponse>> GenerateRefreshTokenAsync(RefreshTokenRequest request)
        {
            RefreshTokenResponse response = new RefreshTokenResponse();

            RefreshToken? refreshToken = await todoContext.RefreshTokens
                .Include(u => u.User)
                .FirstOrDefaultAsync(u => u.Token == request.RefreshToken);

            if (refreshToken is null || refreshToken.Expires < DateTime.UtcNow)
            {
                return Result.Failure<RefreshTokenResponse>(UserErrors.RefreshTokenExpired);
            }

            var accessTokenResponse = await tokenProvider.GenerateAccessToken(refreshToken.User!);
            //new refresh token
            var newRefreshToken = tokenProvider.GenerateRefreshToken(refreshToken.User!.Id);
            //update referesh token
            refreshToken.Token = newRefreshToken.Token;
            refreshToken.Expires = DateTime.UtcNow.AddDays(7);

            await todoContext.SaveChangesAsync();

            response.RefreshToken = newRefreshToken.Token;
            response.AccessToken = accessTokenResponse.AccessToken;
            response.ExpiresIn = accessTokenResponse.ExpiresIn;

            return response;
        }

    }
}
