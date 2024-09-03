using System.Text.Json.Serialization;

namespace tc.Dto
{
    public class UserLoginDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        public UserLoginDto(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public UserLoginDto()
        {
        }
    }
}
