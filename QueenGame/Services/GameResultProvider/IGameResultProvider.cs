namespace QueenGame.Services.GameResultProvider
{
    public interface IGameResultProvider
    {
        public Task<int?> GetBestDuration(int gameId);

        public Task<bool> UpdateBestDuration(int gameId, int? duration);
    }
}
