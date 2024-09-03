using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace tc.Dto
{
    public class JwtAuthenticationResponse
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public int expiresIn { get; set; }
    }
}
