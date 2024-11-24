using QueenGame.DB.DbContexts;
using QueenGame.Game.GameGenerator;
using QueenGame.Game.Models;
using QueenGame.Services.GameInfoProvider;
using QueenGame.Services.GameProvider;
using QueenGame.WPF.Commands;
using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace QueenGame.WPF.ViewModels
{
    public class LevelsListingViewModel : BaseViewModel
    {
        private NavigationStore navigationStore;
        private IGameInfoProvider gameInfoProvider;
        private readonly NavigationService<LevelViewModel> navigationServiceGameLevel;
        private LevelStore levelStore;
        private readonly LevelsInfoStore levelsStore;
        private readonly ObservableCollection<GameInfoViewModel> _GameLevels;
        public IEnumerable<GameInfoViewModel> GameLevels => _GameLevels;

        public ICommand ToMainPageViewModelCommand { get; }
        public ICommand LoadGameLevelsCommand { get; }

        private bool _IsLoading;
        public bool IsLoading 
        { 
            get => _IsLoading;
            set 
            { 
                _IsLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public static LevelsListingViewModel LoadViewModel(
            NavigationStore navigationStore,
            IGameInfoProvider gameProvider,
            NavigationService<MainPageViewModel> navigationServiceMainPage, 
            NavigationService<LevelViewModel> navigationServiceGameLevel,
            LevelStore levelStore,
            LevelsInfoStore levelsStore)
        {
            LevelsListingViewModel viewModel = new LevelsListingViewModel(
                navigationStore,
                gameProvider,
                navigationServiceMainPage,
                navigationServiceGameLevel,
                levelStore,
                levelsStore);
                        
            viewModel.LoadGameLevelsCommand.Execute(null);

            return viewModel;
        }

        private LevelsListingViewModel(
            NavigationStore navigationStore,
            IGameInfoProvider gameProvider,
            NavigationService<MainPageViewModel> navigationServiceMainPage,
            NavigationService<LevelViewModel> navigationServiceGameLevel,
            LevelStore levelStore,
            LevelsInfoStore levelsStore)
        {
            this.navigationStore = navigationStore;
            this.gameInfoProvider = gameProvider;
            this.navigationServiceGameLevel = navigationServiceGameLevel;
            this.levelStore = levelStore;
            this.levelsStore = levelsStore;

            _GameLevels = new ObservableCollection<GameInfoViewModel>();
            IsLoading = false;

            ToMainPageViewModelCommand = new NavigationCommand<MainPageViewModel>(navigationServiceMainPage);
            LoadGameLevelsCommand = new LoadLevelsInfoCommand(this, gameProvider, levelsStore, null);
        }

        public void UpdateGameLevels(IEnumerable<GameInfo> gameLevelInfos) 
        {
            _GameLevels.Clear();
            GameInfoViewModel? viewModel = null;
            foreach (var levelInfo in gameLevelInfos)
            {
                viewModel = new GameInfoViewModel(
                    levelInfo, 
                    navigationServiceGameLevel,
                    levelStore);

                _GameLevels.Add(viewModel);
            }
        }

    }
}
