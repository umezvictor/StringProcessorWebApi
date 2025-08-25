namespace Shared.Settings
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SigninKey { get; set; } = string.Empty;
    }
}
