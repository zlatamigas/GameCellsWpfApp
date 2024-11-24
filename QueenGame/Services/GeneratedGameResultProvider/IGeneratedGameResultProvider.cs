using QueenGame.Game.Models;

namespace QueenGame.Services.GeneratedGameResultProvider
{
    public interface IGeneratedGameResultProvider
    {
        public Task<int?> GetBestDuration(int size);

        public Task UpdateDuration(int size, int? duration);

        public Task<IEnumerable<GeneratedGameResult>> GetGeneratedGameResults();
    }
}
