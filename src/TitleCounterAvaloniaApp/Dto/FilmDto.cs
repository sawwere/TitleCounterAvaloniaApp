using System;
using System.Text.Json.Serialization;

namespace tc.Dto
{
    public class FilmDto : ISearchable
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }


        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("alternative_title")]
        public string? AlternativeTitle { get; set; }


        [JsonPropertyName("imdb_id")]
        public string? ImdbId { get; set; }

        [JsonPropertyName("imdb_id")]
        public string? KpId { get; set; }

        public string? LinkUrl
        {
            get
            {
                if (KpId is not null)
                    return KpId;
                return ImdbId;

            }
        }


        [JsonPropertyName("time")]
        public long? Time { get; set; }


        [JsonPropertyName("date_release")]
        public string? DateRelease { get; set; }


        [JsonPropertyName("global_score")]
        public float? GlobalScore { get; set; }
    }
}
