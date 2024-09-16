using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using tc.Models;

namespace tc.Dto
{
    


    public class GameDto : ISearchable
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("game_type")]
        public string GameType { get; set; } = "game";

        [JsonPropertyName("developer")]
        public List<GameDeveloper> Developers { get; set; } = new List<GameDeveloper>();

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("external_id")]
        public GameExternalId ExternalId { get; set; } = new GameExternalId();

        [JsonPropertyName("platforms")]
        public List<GamePlatform> Platforms { get; set; } = new List<GamePlatform>();


        public string? LinkUrl
        {
            get => ExternalId.HltbId;
        }

        [JsonPropertyName("time")]
        public long? Time { get; set; }


        [JsonPropertyName("date_release")]
        public string? DateRelease { get; set; }


        [JsonPropertyName("global_score")]
        public float? GlobalScore { get; set; }
        public GameDto() { }
    }
}
