using QueenGame.Game.Models;
using QueenGame.WPF.Commands;
using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using System.Windows.Input;

namespace QueenGame.WPF.ViewModels
{
    public class GameInfoViewModel : BaseViewModel
    {
        private GameInfo gameInfo;
        private readonly LevelStore levelStore;

        public int? GameId => gameInfo?.GameId;
        public int? Size => gameInfo?.Size;
        public GameType? GameType  => gameInfo?.GameType;
        public int? FinishedGameDuration => gameInfo?.FinishedGameDuration;

        public ICommand GameStartCommand { get; }

        public GameInfoViewModel(
            GameInfo gameLevelInfo, 
            NavigationService<LevelViewModel> navigationServiceGameLevel,
            LevelStore levelStore)
        {
            this.gameInfo = gameLevelInfo;
            this.levelStore = levelStore;
            GameStartCommand = new ProvideLevelCommand(
                new LevelOption 
                { 
                    GameId = gameLevelInfo?.GameId,
                    Size = gameLevelInfo?.Size,
                },
                navigationServiceGameLevel,
                levelStore);
        }
    }
}
