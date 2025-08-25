using Shared.Enums;

namespace Shared.Requests
{

    public record CreateUserRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRoleType Role { get; set; }
    }
}
