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

        public readonly Uri SearchUri = new Uri($"/films?q=");

        public IEnumerable<FilmDto> SearchByTitle(string title)
        {
            var filmDtos = _restApiClient.GetFromJsonAsync<List<FilmDto>>($"/api/films?q={title}")
                .Result;
            return filmDtos is null ? [] : filmDtos;
        }

        public IEnumerable<FilmEntryResponseDto> FindAll()
        {
            var filmDtos = _restApiClient.GetFromJsonAsync<List<FilmEntryResponseDto>>(
                    $"/api/users/{_userService.GetCurrentUserOrThrow().Username}/films"
                )
                .Result;
            return filmDtos is null ? [] : filmDtos;
        }

        public bool CreateFilmEntry(FilmEntryRequestDto filmEntry)
        {
            var response = _restApiClient.PostJsonAsync($"/api/users/{_userService.GetCurrentUserOrThrow().Username}/films",
                    filmEntry)
                .Result;
            return response.IsSuccessStatusCode;
        }

        public bool DeleteFilmEntry(long id)
        {
            var response = _restApiClient.DeleteAsync($"/api/films/submissions/{id}").Result;
            return response.IsSuccessStatusCode;
        }

        public bool UpdateFilmEntry(FilmEntryRequestDto filmEntryRequestDto)
        {
            var response = _restApiClient.PutJsonAsync($"/api/films/submissions/{filmEntryRequestDto.Id}",
                    filmEntryRequestDto)
                .Result;
            return response.IsSuccessStatusCode;
        }
    }
}
