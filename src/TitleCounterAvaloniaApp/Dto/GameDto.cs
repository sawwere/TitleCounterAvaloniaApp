using System.Text.Json.Serialization;

namespace tc.Dto
{
    public class GameDto : ISearchable
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }


        [JsonPropertyName("title")]
        public string Title { get; set; }


        [JsonPropertyName("link_url")]
        public string? LinkUrl { get; set; }


        [JsonPropertyName("time")]
        public long? Time { get; set; }


        [JsonPropertyName("date_release")]
        public string? DateRelease { get; set; }


        [JsonPropertyName("global_score")]
        public float? GlobalScore { get; set; }
        public GameDto() { }
    }
}
