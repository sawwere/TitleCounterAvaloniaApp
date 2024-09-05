using System;

namespace tc.Models
{
    public class Game : AbstractContent
    {
        public string? HltbId { get; set; }

        public static GameBuilder Builder()
        {
            return new GameBuilder();
        }

        public class GameBuilder
        {
            private readonly Game _game;
            public GameBuilder()
            {
                _game = new Game();
            }

            public GameBuilder Id(long id)
            {
                _game.Id = id;
                return this;
            }

            public GameBuilder Title(string title)
            {
                _game.Title = title;
                return this;
            }

            public GameBuilder DateRelease(DateOnly? date)
            {
                _game.DateRelease = date;
                return this;
            }
            public GameBuilder HltbId(string? hltbId)
            {
                _game.HltbId = hltbId;
                return this;
            }

            public GameBuilder GlobalScore(float? globalScore)
            {
                _game.GlobalScore = globalScore;
                return this;
            }

            public GameBuilder Time(long? globalTime)
            {
                _game.Time = globalTime;
                return this;
            }

            public Game Build()
            {
                return _game;
            }
        }
    }
}
