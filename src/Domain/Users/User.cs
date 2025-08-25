using Microsoft.AspNetCore.Identity;

namespace Domain.Users
{
    public sealed class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();


    }
}
