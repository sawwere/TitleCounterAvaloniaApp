using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace tc.Dto
{
    public class UserDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }
    }
}
