using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using tc.Dto;
using tc.Service;

namespace tc.Repository
{
    public class RestFilmRepository : IFilmRepository
    {
        private readonly UserService _userService;
        private readonly RestApiClient _restApiClient;

        public RestFilmRepository(UserService userService, RestApiClient restApiClient)
        {
            _userService = userService;
            _restApiClient = restApiClient;
        }

        public IEnumerable<FilmDto> SearchByTitle(string title)
        {
            var requestUri = new Uri(_restApiClient.HttpClient.BaseAddress + $"/films?q={title}");
            var filmDtos = _restApiClient.HttpClient.GetFromJsonAsync<List<FilmDto>>(
                    requestUri
                )
                .Result;
            return filmDtos is null ? [] : filmDtos;
        }

        public IEnumerable<FilmEntryResponseDto> FindAll()
        {
            var filmDtos = _restApiClient.HttpClient.GetFromJsonAsync<List<FilmEntryResponseDto>>(
                    _restApiClient.HttpClient.BaseAddress + $"/users/{_userService.GetCurrentUserOrThrow().Username}/films"
                )
                .Result;
            return filmDtos is null ? [] : filmDtos;
        }

        public bool CreateFilmEntry(FilmEntryRequestDto filmEntry)
        {
            var response = _restApiClient.HttpClient.PostAsJsonAsync(
                    _restApiClient.HttpClient.BaseAddress + $"/users/{_userService.GetCurrentUserOrThrow().Username}/films",
                    filmEntry)
                .Result;
            return response.IsSuccessStatusCode;
        }

        public bool DeleteFilmEntry(long id)
        {
            var response = _restApiClient.HttpClient.DeleteAsync(_restApiClient.HttpClient.BaseAddress + $"/films/submissions/{id}").Result;
            return response.IsSuccessStatusCode;
        }

        public bool UpdateFilmEntry(FilmEntryRequestDto filmEntryRequestDto)
        {
            var response = _restApiClient.HttpClient.PutAsJsonAsync(
                    _restApiClient.HttpClient.BaseAddress + $"/films/submissions/{filmEntryRequestDto.Id}",
                    filmEntryRequestDto)
                .Result;
            return response.IsSuccessStatusCode;
        }
    }
}
