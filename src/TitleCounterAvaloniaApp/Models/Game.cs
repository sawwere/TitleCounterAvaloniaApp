using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace tc.Models
{
    public class GamePlatform
    {
        public GamePlatform(long id, string name)
        {
            Id = id;
            Name = name;
        }

        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class GameDeveloper
    {
        public GameDeveloper(long id, string name)
        {
            Id = id;
            Name = name;
        }

        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class GameExternalId
    {
        public GameExternalId()
        {
        }

        public GameExternalId(string hltbId, string steamId)
        {
            HltbId = hltbId;
            SteamId = steamId;
        }

        [JsonPropertyName("hltb_id")]
        public string? HltbId { get; set; }
        [JsonPropertyName("steam_id")]
        public string? SteamId { get; set; }
    }

    public class Game : AbstractContent, ICloneable
    {
        public string GameType { get; set; } = "game";

        public List<GameDeveloper> Developers { get; set; } = new List<GameDeveloper>();

        public string? Description { get; set; }

        public GameExternalId ExternalId { get; private set; } = new GameExternalId();

        public List<GamePlatform> Platforms { get; private set; } = new List<GamePlatform>();

        public static GameBuilder Builder()
        {
            return new GameBuilder();
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            throw new NotImplementedException();
            return Builder()
                .Id(Id)
                .Title(Title)
                //.Developer(Developer)
                .GameType(GameType)
                .ExternalId(ExternalId)
                .DateRelease(DateRelease)
                .GlobalScore(GlobalScore)
                .Time(Time)
                .Build();
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

            public GameBuilder GameType(string gameType)
            {
                _game.GameType = gameType;
                return this;
            }

            public GameBuilder Description(string? descr)
            {
                _game.Description = descr;
                return this;
            }

            public GameBuilder DateRelease(DateOnly? date)
            {
                _game.DateRelease = date;
                return this;
            }
            public GameBuilder ExternalId(GameExternalId externalId)
            {
                _game.ExternalId = externalId;
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
