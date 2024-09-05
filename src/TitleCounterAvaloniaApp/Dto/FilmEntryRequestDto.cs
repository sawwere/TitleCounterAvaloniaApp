using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace tc.Dto
{
    public class FilmEntryRequestDto
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [Length(minimumLength:1, maximumLength:64)]
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

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("film_id")]
        public long FilmId { get; set; }

        public static FilmEntryDtoBuilder Builder() { return new FilmEntryDtoBuilder(); }
    }

    public class FilmEntryDtoBuilder
    {
        private readonly FilmEntryRequestDto _result;
        public FilmEntryDtoBuilder()
        {
            _result = new FilmEntryRequestDto();
        }

        public FilmEntryDtoBuilder Id(long? id)
        {
            _result.Id = id;
            return this;
        }

        public FilmEntryDtoBuilder FilmId(long FilmId)
        {
            _result.FilmId = FilmId;
            return this;
        }

        public FilmEntryDtoBuilder UserId(long userId)
        {
            _result.UserId = userId;
            return this;
        }

        public FilmEntryDtoBuilder CustomTitle(string title)
        {
            _result.CustomTitle = title;
            return this;
        }

        public FilmEntryDtoBuilder DateCompleted(DateOnly? date)
        {
            if (date is not null)
                _result.DateCompleted = $"{date.Value.Year}-{string.Format("{0:D2}", date.Value.Month)}-{string.Format("{0:D2}", date.Value.Day)}";
            return this;
        }

        public FilmEntryDtoBuilder Score(long? score)
        {
            _result.Score = score;
            return this;
        }

        public FilmEntryDtoBuilder Status(string status)
        {
            _result.Status = status;
            return this;
        }

        public FilmEntryDtoBuilder Note(string? note)
        {
            _result.Note = note;
            return this;
        }

        public FilmEntryRequestDto Build()
        {
            return _result;
        }
    }
}
