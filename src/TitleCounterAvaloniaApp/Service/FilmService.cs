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
    public class FilmService /*ISearchableService*/
    {
        private readonly IFilmRepository _filmRepository;
        private readonly RestApiClient _restClient;
        private readonly UserService _userService;

        public FilmService(IFilmRepository filmRepository, RestApiClient restApiClient, UserService userService)
        {
            _filmRepository = filmRepository;
            _restClient = restApiClient;
            _userService = userService;
        }

        public void Create(ISearchable content)
        {
            FilmEntryRequestDto filmEntry = FilmEntryRequestDto.Builder()
                .Id(null)
                .CustomTitle(content.Title)
                .Score(null)
                .Status("backlog")
                .DateCompleted(null)
                .UserId(_userService.GetCurrentUserOrThrow().Id)
                .FilmId(content.Id)
                .Build();

            _filmRepository.CreateFilmEntry(filmEntry);
        }

        public IEnumerable<ISearchable> SearchByTitle(string title)
        {
            return _filmRepository.SearchByTitle(title);
        }

        public async Task<IEnumerable<Entry>> Load()
        {
            return _filmRepository.FindAll().Select(x => DtoToEntry(x));
        }

        public void Remove(long id)
        {
            if (_filmRepository.DeleteFilmEntry(id))
            {
                Debug.WriteLine($"deleted gamesId = {id}");
            }
        }

        public void Update(Entry content)
        {
            if ((GameEntry)content is null)
            {
                throw new InvalidCastException(nameof(content));
            }
            _filmRepository.UpdateFilmEntry(EntryToRequestDto(content as FilmEntry));
        }

        public async Task<Stream> LoadCoverBitmapAsync(long id)
        {
            if (File.Exists(id + ".bmp"))
            {
                return File.OpenRead(id + ".bmp");
            }
            else
            {
                var data = await _restClient.GetByteArrayAsync($@"/api/images/films/{id}.jpg");
                return new MemoryStream(data);
            }
        }

        public static FilmEntry DtoToEntry(FilmEntryResponseDto filmEntryDto)
        {
            if (!DateOnly.TryParse(filmEntryDto.DateCompleted, out DateOnly dateC) && filmEntryDto.DateCompleted is not null)
            {
                throw new JsonParseException($"Error parsing dateCompleted of film with id {filmEntryDto.Film.Id}");
            }
            if (!DateOnly.TryParse(filmEntryDto.Film.DateRelease, out DateOnly dateR) && filmEntryDto.Film.DateRelease is not null)
            {
                throw new JsonParseException($"Error parsing dateRelease of film with id {filmEntryDto.Film.Id}");
            }
            Film film = Film.Builder()
                .Id(filmEntryDto.Film.Id)
                .Title(filmEntryDto.Film.Title)
                .AlternativeTitle(filmEntryDto.Film.AlternativeTitle)
                .DateRelease(dateR)
                .Time(filmEntryDto.Film.Time)
                .LinkUrl(filmEntryDto.Film.LinkUrl)
                .GlobalScore(filmEntryDto.Film.GlobalScore)
                .Build();

            return FilmEntry.Builder()
                .Id(filmEntryDto.Film.Id)
                .Film(film)
                .UserId(filmEntryDto.UserId)
                .CustomTitle(filmEntryDto.CustomTitle)
                .DateCompleted(dateC)
                .Status(filmEntryDto.Status)
                .Score(filmEntryDto.Score)
                .Note(filmEntryDto.Note)
                .Build();
        }

        public static FilmEntryRequestDto EntryToRequestDto(FilmEntry filmEntry)
        {
            return FilmEntryRequestDto.Builder()
                .Id(filmEntry.Id)
                .FilmId(filmEntry.Content.Id)
                .UserId(filmEntry.UserId)
                .CustomTitle(filmEntry.CustomTitle)
                .DateCompleted(filmEntry.DateCompleted)
                .Status(filmEntry.Status)
                .Score(filmEntry.Score)
                .Note(filmEntry.Note)
                .Build();
        }
    }
}
