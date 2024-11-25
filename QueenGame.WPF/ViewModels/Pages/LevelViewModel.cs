using QueenGame.Game.Models;
using QueenGame.Services.GameResultProvider;
using QueenGame.WPF.Commands;
using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using QueenGame.WPF.Utils.Timers;
using QueenGame.WPF.ViewModels.ObjectViewModels;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace QueenGame.WPF.ViewModels
{
    public class LevelViewModel : BaseViewModel
    {
        public RoutedEventHandler PauseClickedEvent { get; set; }

        public ICommand ReturnRequestCommand { get; }
        public ICommand ReturnConfirmCommand { get; }
        public ICommand ReturnDenyCommand { get; }

        public ICommand PauseCommand { get; }
        public ICommand ResumeCommand { get; }
        public ICommand ClearSelectionCommand { get; }

        public ICommand NextLevelCommand { get; }
        public ICommand ReturnAfterFinishCommand { get; }
       

        private LevelStore levelStore;
        private readonly LevelsInfoStore levelsInfoStore;
        private ObservableCollection<CellViewModel> _Cells;
        public IEnumerable<CellViewModel> Cells => _Cells;

        public void OnCellsChanged() 
        {
            FillCellField();
            OnPropertyChanged(nameof(Cells));
        }

        public LevelState LevelState
        {
            get { return levelStore?.CurrentLevelState ?? LevelState.ERROR; }
        }
        public int? GameId => levelStore?.GameId;
        public int? Size => levelStore?.Size;

        private int _Duration;
        public int Duration {
            get { return _Duration; } 
            set 
            { 
                _Duration = value; 
                OnPropertyChanged(nameof(Duration));
            }
        }

        public int? GameBestDuration => levelStore?.GameBestDuration;

        private LevelTimer timer;

        public bool IsPaused => levelStore.CurrentLevelState == LevelState.PAUSED;
        public bool IsCompleted => levelStore.CurrentLevelState == LevelState.FINISHED 
                                    && levelStore.IsCompleted;
        public bool IsLoading => levelStore.CurrentLevelState == LevelState.LOADING;
        
        private bool _IsRequestedBack;
        public bool IsRequestedBack
        {
            get { return  _IsRequestedBack; }
            set 
            { 
                _IsRequestedBack = value; 
                OnPropertyChanged(nameof(IsRequestedBack));
            }
        }

        public bool IsGeneratedLevel => GameId == null;

        public override void Dispose()
        {
            levelStore.CurrentLevelStateChanged -= OnCurrentLevelStateChanged;
            levelStore.GameBestDurationChanged -= OnGameBestDurationChanged;
            timer.Elapsed -= OnTimerChanged;
            base.Dispose();
        }

        private LevelViewModel(
            LevelStore levelStore,
            LevelsInfoStore levelsInfoStore,
            NavigationService<GenerateLevelOptionsViewModel> navigationServiceGenerateLevelOptionsViewModel,
            NavigationService<LevelsListingViewModel> navigationServiceLevelsListingViewModel,
            NavigationService<LevelViewModel> navigationServiceLevelViewModel
            )
        {
            this.levelStore = levelStore;
            this.levelsInfoStore = levelsInfoStore;
            levelStore.CurrentLevelStateChanged += OnCurrentLevelStateChanged;
            levelStore.GameBestDurationChanged += OnGameBestDurationChanged;

            _Cells = new ObservableCollection<CellViewModel>();

            timer = new LevelTimer(1000);
            timer.Elapsed += OnTimerChanged;

            ReturnRequestCommand = new BackFromLevelRequestCommand(this, levelStore);
            ReturnDenyCommand = new BackFromLevelDenyCommand(this);
            if (GameId != null)
            {
                ReturnConfirmCommand = new BackFromLevelConfirmCommand<LevelsListingViewModel>(this, levelStore, navigationServiceLevelsListingViewModel);
            }
            else if (Size != null)
            {
                ReturnConfirmCommand = new BackFromLevelConfirmCommand<GenerateLevelOptionsViewModel>(this, levelStore, navigationServiceGenerateLevelOptionsViewModel);
            }
            
            PauseCommand = new PauseCommand(this, levelStore);
            ResumeCommand = new ResumeCommand(this, levelStore);
            ClearSelectionCommand = new ClearSelectionCommand(this, levelStore);

            NextLevelCommand = new NextLevelCommand(this, levelStore, levelsInfoStore, navigationServiceLevelViewModel);
        }

        private void OnGameBestDurationChanged()
        {
            OnPropertyChanged(nameof(GameBestDuration));
        }

        public static LevelViewModel LoadViewModel(
            LevelStore gameInfoStore,
            LevelsInfoStore levelsInfoStore,
            NavigationService<GenerateLevelOptionsViewModel> navigationServiceGenerateLevelOptionsViewModel,
            NavigationService<LevelsListingViewModel> navigationServiceLevelsListingViewModel,
            NavigationService<LevelViewModel> navigationServiceLevelViewModel)
        {
            LevelViewModel viewModel = new LevelViewModel(
                gameInfoStore,
                levelsInfoStore,
                navigationServiceGenerateLevelOptionsViewModel,
                navigationServiceLevelsListingViewModel,
                navigationServiceLevelViewModel
            );

            viewModel.OnCurrentLevelStateChanged();

            return viewModel;
        }

        public void FillCellField() 
        {

            List<CellViewModel> cells = levelStore.CurrentGameCellField
                            .Select(v => new CellViewModel(v, levelStore, this))
                            .ToList();

            _Cells.Clear();
            foreach (var cell in cells)
            {
                _Cells.Add(cell);
            }
        }

        private async void OnCurrentLevelStateChanged()
        {
            switch (levelStore.CurrentLevelState)
            {
                case LevelState.LOADING:
                    OnPropertyChanged(nameof(IsLoading));
                    break;
                case LevelState.LOADED:
                    FillCellField();
                    OnCellsChanged();
                    Duration = 0;
                    OnPropertyChanged(nameof(IsLoading));
                    OnPropertyChanged(nameof(IsCompleted));
                    break;
                case LevelState.STARTED:
                    OnPropertyChanged(nameof(IsPaused));
                    timer.Start();
                    break;
                case LevelState.FINISHED:
                    if (levelStore.IsCompleted)
                    {
                        if (GameBestDuration == null || GameBestDuration > Duration)
                        {
                            levelStore.GameBestDuration = Duration;
                            if (GameId != null)
                            {
                                levelsInfoStore.UpdateLevelOptionDuration(GameId.Value, Duration);
                            }
                        }
                        
                        OnPropertyChanged(nameof(IsCompleted));
                    }
                    timer.Stop();
                    break;
                case LevelState.PAUSED:
                    timer.Stop();
                    OnPropertyChanged(nameof(IsPaused));
                    break;
            }
            OnPropertyChanged(nameof(LevelState));
        }

        private void OnTimerChanged(object? sender, ElapsedEventArgs e)
        {
            switch (levelStore.CurrentLevelState) 
            { 
                case LevelState.STARTED:
                    Duration += 1;
                    break;
                case LevelState.NOT_READY:
                case LevelState.LOADING:
                case LevelState.LOADED:
                case LevelState.FINISHED:
                case LevelState.PAUSED:
                case LevelState.ERROR:
                    break;
            }
        }
    }
}
