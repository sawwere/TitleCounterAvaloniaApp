using System;

namespace tc.Models;

public partial class FilmEntry : Entry
{
    public new Film Content { get; set; }
    public static FilmEntryBuilder Builder()
    {
        return new FilmEntryBuilder();
    }

    public class FilmEntryBuilder
    {
        private readonly FilmEntry _res;
        public FilmEntryBuilder()
        {
            _res = new FilmEntry();
        }

        public FilmEntryBuilder Id(long id)
        {
            _res.Id = id;
            return this;
        }

        public FilmEntryBuilder Film(Film film)
        {
            _res.Content = film;
            return this;
        }

        public FilmEntryBuilder UserId(long userId)
        {
            _res.UserId = userId;
            return this;
        }


        public FilmEntryBuilder CustomTitle(string title)
        {
            _res.CustomTitle = title;
            return this;
        }

        public FilmEntryBuilder DateCompleted(DateOnly? date)
        {
            _res.DateCompleted = date;
            return this;
        }

        public FilmEntryBuilder Score(long? score)
        {
            _res.Score = score;
            return this;
        }

        public FilmEntryBuilder Status(string status)
        {
            _res.Status = status;
            return this;
        }

        public FilmEntryBuilder Note(string? note)
        {
            _res.Note = note;
            return this;
        }

        public FilmEntry Build()
        {
            return _res;
        }
    }
}
