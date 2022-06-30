using System.Text.Json.Serialization;

namespace Marc2.Contracts.Auth
{
    public class SimpleTokenResponse
    {
        [JsonPropertyName("token")]
        public string JwtToken { get; set; } = null!;
        [JsonPropertyName("expiresAfter")]
        public int ExpiresAfter { get; set; }
    }
}
