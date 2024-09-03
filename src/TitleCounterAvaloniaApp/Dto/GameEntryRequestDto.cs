using System;
using System.Text.Json.Serialization;

namespace tc.Dto
{
    public class GameEntryRequestDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }


        [JsonPropertyName("custom_title")]
        public string CustomTitle { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("score")]
        public long Score { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("date_completed")]
        public string DateCompleted { get; set; }

        [JsonPropertyName("time")]
        public long Time { get; set; }

        [JsonPropertyName("platform")]
        public string? Platform { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("game_id")]
        public long GameId { get; set; }

        public static GameEntryDtoBuilder Builder() { return new GameEntryDtoBuilder(); }

        public class GameEntryDtoBuilder
        {
            private readonly GameEntryRequestDto _result;
            public GameEntryDtoBuilder()
            {
                _result = new GameEntryRequestDto();
            }

            public GameEntryDtoBuilder Id(long id)
            {
                _result.Id = id;
                return this;
            }

            public GameEntryDtoBuilder GameId(long gameId)
            {
                _result.GameId = gameId;
                return this;
            }

            public GameEntryDtoBuilder UserId(long userId)
            {
                _result.UserId = userId;
                return this;
            }

            public GameEntryDtoBuilder CustomTitle(string title)
            {
                _result.CustomTitle = title;
                return this;
            }

            public GameEntryDtoBuilder DateCompleted(DateOnly date)
            {
                _result.DateCompleted = $"{date.Year}-{string.Format("{0:D2}", date.Month)}-{string.Format("{0:D2}", date.Day)}";
                return this;
            }

            public GameEntryDtoBuilder Platform(string? platform)
            {
                _result.Platform = platform;
                return this;
            }

            public GameEntryDtoBuilder Score(long score)
            {
                _result.Score = score;
                return this;
            }

            public GameEntryDtoBuilder Status(string status)
            {
                _result.Status = status;
                return this;
            }

            public GameEntryDtoBuilder Time(long time)
            {
                _result.Time = time;
                return this;
            }

            public GameEntryDtoBuilder Note(string? note)
            {
                _result.Note = note;
                return this;
            }

            public GameEntryRequestDto Build()
            {
                return _result;
            }
        }
    }
}
