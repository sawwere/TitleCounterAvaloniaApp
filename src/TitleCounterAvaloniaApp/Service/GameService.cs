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

        public async Task<Entry?> Create(ISearchable content)
        {
            GameEntryRequestDto gameEntry = GameEntryRequestDto.Builder()
                .Id(null)
                .CustomTitle(content.Title)
                .Score(null)
                .Status("backlog")
                .DateCompleted(null)
                .Time(null)
                .UserId(_userService.GetCurrentUserOrThrow().Id)
                .GameId(content.Id)
                .Build();
            var response = await _gameRepository.CreateGameEntry(gameEntry);
            return DtoToEntry(response); //TODO
        }

        public IEnumerable<ISearchable> SearchByTitle(string title)
        {
            return _gameRepository.SearchByTitle(title);
        }

        public async Task<IEnumerable<Entry>> FindAll()
        {
            return (await _gameRepository.FindAll()).Select(x => DtoToEntry(x));
        }

        public void Remove(long id)
        {
            if (_gameRepository.DeleteGameEntry(id).Result)
            {
                Debug.WriteLine($"deleted gamesId = {id}");
            }
        }

        public Task<GameEntryResponseDto?> Update(GameEntry entry)
        {
            ArgumentNullException.ThrowIfNull(entry);
            return _gameRepository.UpdateGameEntry(EntryToRequestDto(entry));
        }

        public async Task<byte[]> LoadCoverAsync(long id)
        {
            var data = await _restClient.GetByteArrayAsync($"/api/images/games/{id}.jpg");
            return data;
        }

        public static GameEntry DtoToEntry(GameEntryResponseDto gameEntryDto)
        {
            if (!DateOnly.TryParse(gameEntryDto.DateCompleted, out DateOnly dateC) && gameEntryDto.DateCompleted is not null)
            {
                throw new JsonParseException($"Error parsing dateCompleted of game with id {gameEntryDto.Game.Id}");
            }
            if (!DateOnly.TryParse(gameEntryDto.Game.DateRelease, out DateOnly dateR) && gameEntryDto.Game.DateRelease is not null)
            {
                throw new JsonParseException($"Error parsing dateRelease of game with id {gameEntryDto.Game.Id}");
            }
            Game game = Game.Builder()
                .Id(gameEntryDto.Game.Id)
                .Title(gameEntryDto.Game.Title)
                .DateRelease(dateR)
                .Time(gameEntryDto.Game.Time)
                .HltbId(gameEntryDto.Game.HltbId)
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
