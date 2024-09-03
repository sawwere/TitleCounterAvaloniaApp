using System;

namespace tc.Models
{
    public class Film : AbstractContent
    {
        public string? RusTitle { get; set; }

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

            public FilmBuilder RusTitle(string title)
            {
                _film.RusTitle = title;
                return this;
            }

            public FilmBuilder DateRelease(DateOnly date)
            {
                _film.DateRelease = date;
                return this;
            }
            public FilmBuilder LinkUrl(string? linkUrl)
            {
                _film.LinkUrl = linkUrl;
                return this;
            }

            public FilmBuilder GlobalScore(float? globalScore)
            {
                _film.GlobalScore = globalScore;
                return this;
            }

            public FilmBuilder Time(long globalTime)
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
