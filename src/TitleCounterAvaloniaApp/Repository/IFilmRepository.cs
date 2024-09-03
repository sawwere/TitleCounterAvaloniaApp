using System.Collections.Generic;
using tc.Dto;

namespace tc.Repository
{
    public interface IFilmRepository
    {
        public IEnumerable<FilmDto> SearchByTitle(string title);

        public IEnumerable<FilmEntryResponseDto> FindAll();

        public bool CreateFilmEntry(FilmEntryRequestDto filmEntryRequestDto);

        public bool UpdateFilmEntry(FilmEntryRequestDto filmEntryRequestDto);

        public bool DeleteFilmEntry(long id);
    }
}
