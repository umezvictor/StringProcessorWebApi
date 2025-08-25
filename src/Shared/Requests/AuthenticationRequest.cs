using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{

    public record AuthenticationRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
