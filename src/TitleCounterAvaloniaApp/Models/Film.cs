using System;

namespace tc.Models
{
    public class Film : AbstractContent
    {
        public string? AlternativeTitle { get; set; }
        public string? ImdbId { get; set; }
        public string? KpId { get; set; }

        public static FilmBuilder Builder()
        {
            return new FilmBuilder();
        }

        public class FilmBuilder
        {
            private readonly Film _film;
            public FilmBuilder()
            {
                _film = new Film();
            }

            public FilmBuilder Id(long id)
            {
                _film.Id = id;
                return this;
            }

            public FilmBuilder Title(string title)
            {
                _film.Title = title;
                return this;
            }

            public FilmBuilder AlternativeTitle(string? title)
            {
                _film.AlternativeTitle = title;
                return this;
            }

            public FilmBuilder DateRelease(DateOnly date)
            {
                _film.DateRelease = date;
                return this;
            }
            public FilmBuilder ImdbId(string? imdb)
            {
                _film.ImdbId = imdb;
                return this;
            }

            public FilmBuilder LinkUrl(string? kp)
            {
                _film.KpId = kp;
                return this;
            }

            public FilmBuilder GlobalScore(float? globalScore)
            {
                _film.GlobalScore = globalScore;
                return this;
            }

            public FilmBuilder Time(long? globalTime)
            {
                _film.Time = globalTime;
                return this;
            }

            public Film Build()
            {
                return _film;
            }
        }
    }
}
