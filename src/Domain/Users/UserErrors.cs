using Shared;

namespace Domain.Users
{
    public static class UserErrors
    {

        public static readonly Error InvalidCredentials = Error.Failure(
            "Users.InvalidCredentials",
            "Invalid login credentials");

        public static readonly Error EmailNotVerified = Error.Failure(
           "Users.EmailNotVerified",
           "Your email has not been verified.");

        public static readonly Error RefreshTokenExpired = Error.Failure(
           "Users.RefreshTokenExpired",
           "The refresh token has expired");


    }
}


