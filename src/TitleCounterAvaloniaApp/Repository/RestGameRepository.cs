using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using tc.Dto;
using tc.Service;

namespace tc.Repository
{
    public class RestGameRepository : IGameRepository
    {
        private readonly UserService _userService;
        private readonly RestApiClient _restApiClient;

        public RestGameRepository(RestApiClient restApiClient, UserService userService)
        {
            _restApiClient = restApiClient;
            _userService = userService;
        }

        public IEnumerable<GameDto> SearchByTitle(string title)
        {
            var requestUri = new Uri(_restApiClient.HttpClient.BaseAddress + $"/games?q={title}");
            var gameDtos = _restApiClient.HttpClient.GetFromJsonAsync<List<GameDto>>(requestUri)
                .Result;
            return gameDtos is null ? new List<GameDto>() : gameDtos;
        }

        public IEnumerable<GameEntryResponseDto> FindAll()
        {
            var gameDtos = _restApiClient.HttpClient.GetFromJsonAsync<List<GameEntryResponseDto>>(
                    _restApiClient.HttpClient.BaseAddress + $"/users/{_userService.GetCurrentUserOrThrow().Username}/games"
                )
                .Result;
            return gameDtos is null ? new List<GameEntryResponseDto>() : gameDtos;
        }

        public async Task<GameEntryResponseDto?> CreateGameEntry(GameEntryRequestDto gameEntryRequestDto)
        {
            var response = await _restApiClient.HttpClient.PostAsJsonAsync(
                    _restApiClient.HttpClient.BaseAddress + $"/users/{_userService.GetCurrentUserOrThrow().Username}/games",
                    gameEntryRequestDto);
            return JsonConvert.DeserializeObject<GameEntryResponseDto>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<GameEntryResponseDto?> UpdateGameEntry(GameEntryRequestDto gameEntryRequestDto)
        {
            var response = await _restApiClient.HttpClient.PutAsJsonAsync(
                    _restApiClient.HttpClient.BaseAddress + $"/games/submissions/{gameEntryRequestDto.Id}",
                    gameEntryRequestDto);
            return JsonConvert.DeserializeObject<GameEntryResponseDto>(response.Content.ReadAsStringAsync().Result);
        }

        public bool DeleteGameEntry(long id)
        {
            var response = _restApiClient.HttpClient.DeleteAsync(_restApiClient.HttpClient.BaseAddress + $"/games/submissions/{id}").Result;
            return response.IsSuccessStatusCode;
        }
    }
}
