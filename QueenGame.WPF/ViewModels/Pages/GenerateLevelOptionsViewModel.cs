using QueenGame.Game.GameGenerator;
using QueenGame.Game.Models;
using QueenGame.Services.GameInfoProvider;
using QueenGame.Services.GeneratedGameResultProvider;
using QueenGame.WPF.Commands;
using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace QueenGame.WPF.ViewModels
{
    public class GenerateLevelOptionsViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        private string _Size;
        public string Size 
        { 
            get { return _Size; }
            set 
            { 
                _Size = value;

                ClearErrors(nameof(Size));
                string? valueStr = (string?)value;
                if (valueStr == null
                    || valueStr.Length == 0
                    || valueStr.Length > 2
                    || !Int32.TryParse(valueStr, out int number)
                    || number > 25
                    || number < 2
                    || !gameGenerator.CanGenerateGameLevel(number))
                {
                    AddErrors(nameof(Size), $"Illegal number {valueStr}");
                }

                OnPropertyChanged(nameof(Size));
            }
        }

        private ObservableCollection<GeneratedGameResult> _GeneratedGameResults;
        public IEnumerable<GeneratedGameResult> GeneratedGameResults => _GeneratedGameResults;

        public ICommand GenerateGameLevelCommand { get; }
        public ICommand ToMainPageViewModelCommand { get; }
        public ICommand LoadGeneratedGameResultsCommand { get; }


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

        public bool HasResults => GeneratedGameResults.Any();

        private readonly NavigationStore navigationStore;
        private readonly LevelStore levelStore;
        private readonly IGameGenerator gameGenerator;
        private readonly IGeneratedGameResultProvider generatedGameResultProvider;

        private GenerateLevelOptionsViewModel(
            NavigationStore navigationStore,
            NavigationService<MainPageViewModel> navigationServiceMainPageViewModel,
            NavigationService<LevelViewModel> navigationServiceLevelViewModel,
            IGameGenerator gameGenerator,
            LevelStore levelStore,
            IGeneratedGameResultProvider generatedGameResultProvider)
        {
            propertyNameToErrorDict = new Dictionary<string, List<string>>();

            this.navigationStore = navigationStore;
            this.levelStore = levelStore;
            this.generatedGameResultProvider = generatedGameResultProvider;
            this.gameGenerator = gameGenerator;

            ToMainPageViewModelCommand = new NavigationCommand<MainPageViewModel>(navigationServiceMainPageViewModel);

            GenerateGameLevelCommand = new GenerateLevelCommand(
                this,
                navigationServiceLevelViewModel,
                levelStore
                );

            LoadGeneratedGameResultsCommand = new LoadGeneratedGameResultsCommand(this, generatedGameResultProvider, null);

            Size = "7";
            IsLoading = false;

            _GeneratedGameResults = new ObservableCollection<GeneratedGameResult>();
        }
        public void UpdateGeneratedGameResults(IEnumerable<GeneratedGameResult> gameLevelInfos)
        {
            _GeneratedGameResults.Clear();

            foreach (var levelInfo in gameLevelInfos.OrderBy(v => -v.Size))
            {
                _GeneratedGameResults.Add(levelInfo);
            }
            OnPropertyChanged(nameof(HasResults));
        }

        public static GenerateLevelOptionsViewModel LoadViewModel(
            NavigationStore navigationStore,
            NavigationService<MainPageViewModel> navigationServiceMainPageViewModel,
            NavigationService<LevelViewModel> navigationServiceLevelViewModel,
            IGameGenerator gameGenerator,
            LevelStore levelStore,
            IGeneratedGameResultProvider generatedGameResultProvider)
        {
            GenerateLevelOptionsViewModel viewModel = new GenerateLevelOptionsViewModel(
                navigationStore, 
                navigationServiceMainPageViewModel,
                navigationServiceLevelViewModel,
                gameGenerator,
                levelStore,
                generatedGameResultProvider);

            viewModel.LoadGeneratedGameResultsCommand.Execute(null);

            return viewModel;
        }


        private readonly Dictionary<string, List<string>> propertyNameToErrorDict;
        public bool HasErrors => propertyNameToErrorDict.Any();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private void OnErrorsChanged(string propertyName) 
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return propertyName != null ? propertyNameToErrorDict.GetValueOrDefault(propertyName, new List<string>()) : new List<string>();
        }

        public void ClearErrors(string propertyName) 
        {
            propertyNameToErrorDict.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        public void AddErrors(string propertyName, string error)
        {
            if (!propertyNameToErrorDict.ContainsKey(propertyName)) 
            { 
                propertyNameToErrorDict.Add(propertyName, new List<string>());
            }

            propertyNameToErrorDict[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }
}
