using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public record RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
