using System;

namespace tc.Models;

public partial class GameEntry : Entry, ICloneable
{
    public long? Time { get; set; }
    public string? Platform { get; set; }

    public new Game Content { get; set; } 

    public static GameEntryBuilder Builder()
    {
        return new GameEntryBuilder();
    }

    public object Clone()
    {
        return Builder()
            .Id(Id)
            .UserId(UserId)
            .Game(this.Content as Game)
            .CustomTitle(CustomTitle)
            .Status(Status)
            .Time(this.Time)
            .Platform(this.Platform)
            .Note(this.Note)
            .DateCompleted(this.DateCompleted)
            .Score(this.Score)
            .Build();
    }

    public class GameEntryBuilder
    {
        private readonly GameEntry _game;
        public GameEntryBuilder()
        {
            _game = new GameEntry();
        }

        public GameEntryBuilder Id(long id)
        {
            _game.Id = id;
            return this;
        }

        public GameEntryBuilder Game(Game game)
        {
            _game.Content = game;
            return this;
        }

        public GameEntryBuilder UserId(long userId)
        {
            _game.UserId = userId;
            return this;
        }


        public GameEntryBuilder CustomTitle(string title)
        {
            _game.CustomTitle = title;
            return this;
        }

        public GameEntryBuilder DateCompleted(DateOnly? date)
        {
            _game.DateCompleted = date;
            return this;
        }

        public GameEntryBuilder Platform(string? platform)
        {
            _game.Platform = platform;
            return this;
        }

        public GameEntryBuilder Score(long? score)
        {
            _game.Score = score;
            return this;
        }

        public GameEntryBuilder Status(string status)
        {
            _game.Status = status;
            return this;
        }

        public GameEntryBuilder Time(long? time)
        {
            _game.Time = time;
            return this;
        }

        public GameEntryBuilder Note(string? note)
        {
            _game.Note = note;
            return this;
        }

        public GameEntry Build()
        {
            return _game;
        }
    }
}
