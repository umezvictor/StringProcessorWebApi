using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public record CancelJobRequest
    {
        [Required]
        public string JobId { get; set; } = string.Empty;
    }
}
