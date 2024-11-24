using QueenGame.Game.Models;

namespace QueenGame.Services.GameInfoProvider
{
    public interface IGameInfoProvider
    {
        public Task<GameInfo?> GetGameInfo(int gameId);

        public Task<IEnumerable<GameInfo>> GetGameInfoAll();
    }
}
