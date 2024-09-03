using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tc.Dto;
using tc.Models;
using tc.Repository;
using tc.Utils.Exception;

namespace tc.Service
{
    public class GameService : ISearchableService
    {
        private readonly IGameRepository _gameRepository;
        private readonly RestApiClient _restClient;
        private readonly UserService _userService;

        public GameService(IGameRepository gameRepository, RestApiClient restApiClient, UserService userService)
        {
            _gameRepository = gameRepository;
            _userService = userService;
            _restClient = restApiClient;
        }

        public Task<GameEntryResponseDto?> Create(ISearchable content)
        {
            GameEntryRequestDto gameEntry = GameEntryRequestDto.Builder()
                .CustomTitle(content.Title)
                .Score(0)
                .Status("backlog")
                .DateCompleted(DateOnly.FromDateTime(DateTime.Today))
                .Time(content.Time.Value) //TODO
                .UserId(_userService.GetCurrentUserOrThrow().Id)
                .GameId(content.Id)
                .Build();
            return _gameRepository.CreateGameEntry(gameEntry);
        }

        public IEnumerable<ISearchable> SearchByTitle(string title)
        {
            return _gameRepository.SearchByTitle(title);
        }

        public async Task<IEnumerable<Entry>> Load()
        {
            return _gameRepository.FindAll().Select(x => DtoToEntry(x));
        }

        public void Remove(long id)
        {
            if (_gameRepository.DeleteGameEntry(id))
            {
                Debug.WriteLine($"deleted gamesId = {id}");
            }
        }

        public string ToString()
        {
            return "games";
        }

        public Task<GameEntryResponseDto?> UpdateEntry(GameEntry entry)
        {
            ArgumentNullException.ThrowIfNull(entry);
            return _gameRepository.UpdateGameEntry(EntryToRequestDto(entry));
        }

        public async Task<Stream> LoadCoverBitmapAsync(long id)
        {
            if (File.Exists(id + ".bmp"))
            {
                return File.OpenRead(id + ".bmp");
            }
            else
            {
                var data = await _restClient.HttpClient.GetByteArrayAsync($@"http://localhost:8080/images/games/{id}.jpg");
                return new MemoryStream(data);
            }
        }

        public static GameEntry DtoToEntry(GameEntryResponseDto gameEntryDto)
        {
            if (!DateOnly.TryParse(gameEntryDto.DateCompleted, out DateOnly dateC))
            {
                throw new JsonParseException($"Error parsing dateCompleted of game with id {gameEntryDto.Game.Id}");
            }
            if (!DateOnly.TryParse(gameEntryDto.Game.DateRelease, out DateOnly dateR))
            {
                throw new JsonParseException($"Error parsing dateRelease of game with id {gameEntryDto.Game.Id}");
            }
            Game game = Game.builder()
                .Id(gameEntryDto.Game.Id)
                .Title(gameEntryDto.Game.Title)
                .DateRelease(dateR)
                .Time(gameEntryDto.Game.Time.Value)
                .LinkUrl(gameEntryDto.Game.LinkUrl)
                .GlobalScore(gameEntryDto.Game.GlobalScore)
                .Build();
            return GameEntry.Builder()
                .Id(gameEntryDto.Id)
                .Game(game)
                .UserId(gameEntryDto.UserId)
                .CustomTitle(gameEntryDto.CustomTitle)
                .DateCompleted(dateC)
                .Time(gameEntryDto.Time)
                .Status(gameEntryDto.Status)
                .Score(gameEntryDto.Score)
                .Note(gameEntryDto.Note)
                .Platform(gameEntryDto.Platform)
                .Build();
        }

        public static GameEntryRequestDto EntryToRequestDto(GameEntry gameEntry)
        {
            return GameEntryRequestDto.Builder()
                .Id(gameEntry.Id)
                .GameId(gameEntry.Content.Id)
                .UserId(gameEntry.UserId)
                .CustomTitle(gameEntry.CustomTitle)
                .DateCompleted(gameEntry.DateCompleted)
                .Time(gameEntry.Time)
                .Status(gameEntry.Status)
                .Score(gameEntry.Score)
                .Note(gameEntry.Note)
                .Platform(gameEntry.Platform)
                .Build();
        }
    }
}
