using QueenGame.Game.Models;

namespace QueenGame.Services.GameProvider
{
    public interface IGameProvider
    {
        public Task<Game.Models.Game?> GetGameLevelAsync(int gameId);
    }
}
