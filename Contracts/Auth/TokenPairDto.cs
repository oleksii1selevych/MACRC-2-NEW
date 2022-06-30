using System.Text.Json.Serialization;

namespace Marc2.Contracts.Auth
{
    public class TokenPairDto
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = null!;
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = null!;

        [JsonPropertyName("expiresAfter")]
        public int ExpiresAfter { get; set; }
    }
}
