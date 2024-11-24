namespace QueenGame.Game.Models
{
    public class GameInfo
    {
        public int? GameId { get; }
        public int Size { get; }
        public GameType GameType { get; }
        public int? FinishedGameDuration { get; set; }

        public GameInfo(int? gameId, int size, GameType gameType, int? finishedGameDuration)
        {
            GameId = gameId;
            Size = size;
            GameType = gameType;
            FinishedGameDuration = finishedGameDuration;
        }
    }
}
