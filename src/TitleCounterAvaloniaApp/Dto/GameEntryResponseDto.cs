using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace tc.Dto
{
    public class GameEntryResponseDto
    {

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [Length(minimumLength: 1, maximumLength: 64)]
        [JsonPropertyName("custom_title")]
        public string CustomTitle { get; set; }

        [Length(minimumLength: 0, maximumLength: 512)]
        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("score")]
        public long? Score { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("date_completed")]
        public string? DateCompleted { get; set; }

        [JsonPropertyName("time")]
        public long? Time { get; set; }

        [JsonPropertyName("platform")]
        public string? Platform { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("game")]
        public GameDto Game { get; set; }

        public static GameEntryDtoBuilder builder() { return new GameEntryDtoBuilder(); }

        public class GameEntryDtoBuilder
        {
            private GameEntryResponseDto _result;
            public GameEntryDtoBuilder()
            {
                _result = new GameEntryResponseDto();
            }

            public GameEntryDtoBuilder id(long id)
            {
                _result.Id = id;
                return this;
            }

            public GameEntryDtoBuilder gameId(GameDto gameDto)
            {
                _result.Game = gameDto;
                return this;
            }

            public GameEntryDtoBuilder userId(long userId)
            {
                _result.UserId = userId;
                return this;
            }

            public GameEntryDtoBuilder customTitle(string title)
            {
                _result.CustomTitle = title;
                return this;
            }

            public GameEntryDtoBuilder dateCompleted(DateOnly date)
            {
                _result.DateCompleted = $"{date.Year}-{date.Month}-{date.Day}";
                return this;
            }

            public GameEntryDtoBuilder platform(string platform)
            {
                _result.Platform = platform;
                return this;
            }

            public GameEntryDtoBuilder score(long score)
            {
                _result.Score = score;
                return this;
            }

            public GameEntryDtoBuilder status(string status)
            {
                _result.Status = status;
                return this;
            }

            public GameEntryDtoBuilder time(long time)
            {
                _result.Time = time;
                return this;
            }

            public GameEntryDtoBuilder note(string note)
            {
                _result.Note = note;
                return this;
            }

            public GameEntryResponseDto build()
            {
                return _result;
            }
        }
    }
}
