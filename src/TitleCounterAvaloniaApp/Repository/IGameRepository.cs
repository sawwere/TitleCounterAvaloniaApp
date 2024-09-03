using System.Collections.Generic;
using System.Threading.Tasks;
using tc.Dto;

namespace tc.Repository
{
    public interface IGameRepository
    {
        public IEnumerable<GameDto> SearchByTitle(string title);

        public IEnumerable<GameEntryResponseDto> FindAll();

        public Task<GameEntryResponseDto?> CreateGameEntry(GameEntryRequestDto gameEntryRequestDto);

        public Task<GameEntryResponseDto?> UpdateGameEntry(GameEntryRequestDto gameEntryRequestDto);

        public bool DeleteGameEntry(long id);
    }
}
