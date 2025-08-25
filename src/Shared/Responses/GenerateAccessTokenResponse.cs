namespace Shared.Responses
{
    public class GenerateAccessTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }

    }
}
