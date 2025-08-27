using System.ComponentModel.DataAnnotations;

namespace Domain.Procesor
{
    public sealed class ProcessStringRequest
    {

        [Key]
        public string Id { get; set; } = string.Empty;
        public string InputString { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public bool IsCancelled { get; set; }

    }
}
