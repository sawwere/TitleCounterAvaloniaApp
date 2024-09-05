using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using tc.Dto;
using tc.Service;
using tc.Utils.Exception;

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

        private TResult MakeRequest<TValue, TResult>(Func<TValue, TResult> action, TValue value) {
            try
            {
                return action(value);
            }
            catch (HttpRequestException httpEx)
            {
                throw new ServiceUnavailableException(httpEx.Message);
            }
        }

        private async Task<T> MakeRequest<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (HttpRequestException httpEx)
            {
                throw new ServiceUnavailableException(httpEx.Message);
            }
        }


        public IEnumerable<GameDto> SearchByTitle(string title)
        {
            try
            {
                return MakeRequest(title =>
                {
                    var gameDtos = _restApiClient.GetFromJsonAsync<List<GameDto>>($"/api/games?q={title}")
                    .Result;
                    return gameDtos is null ? new List<GameDto>() : gameDtos;
                }, title);
            }
            catch (HttpRequestException httpEx)
            {
                throw new ServiceUnavailableException(httpEx.Message);
            }
        }

        public async Task<IEnumerable<GameEntryResponseDto>> FindAll()
        {

            return await MakeRequest(async () =>
            {
                var gameDtos = await _restApiClient.GetFromJsonAsync<List<GameEntryResponseDto>>($"/api/users/{_userService.GetCurrentUserOrThrow().Username}/games");
                return gameDtos is null ? new List<GameEntryResponseDto>() : gameDtos;
            });
        }

        public async Task<GameEntryResponseDto?> CreateGameEntry(GameEntryRequestDto gameEntryRequestDto)
        {
            return await MakeRequest(async () => {
                var response = await _restApiClient.PostJsonAsync($"/api/users/{_userService.GetCurrentUserOrThrow().Username}/games",
                    gameEntryRequestDto);
                return JsonConvert.DeserializeObject<GameEntryResponseDto>(response.Content.ReadAsStringAsync().Result);
            });
            
        }

        public async Task<GameEntryResponseDto?> UpdateGameEntry(GameEntryRequestDto gameEntryRequestDto)
        {
            return await MakeRequest(async () => {
                var response = await _restApiClient.PutJsonAsync($"/api/games/submissions/{gameEntryRequestDto.Id}",
                    gameEntryRequestDto);
                return JsonConvert.DeserializeObject<GameEntryResponseDto>(response.Content.ReadAsStringAsync().Result);
            });
            
        }

        public async Task<bool> DeleteGameEntry(long id)
        {
            return await MakeRequest(async () =>
            {
                var response = await _restApiClient.DeleteAsync($"/api/games/submissions/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            });
        }
    }
}
