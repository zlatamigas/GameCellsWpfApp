
using QueenGame.Game.GameGenerator;
using QueenGame.Game.Models;
using QueenGame.Services.GameProvider;
using QueenGame.Services.GameResultProvider;
using QueenGame.Services.GeneratedGameResultProvider;
using System.Drawing;

namespace QueenGame.WPF.Stores
{
    public enum LevelState 
    { 
        NOT_READY,  // No game
        
        LOADING,    // Loading/generating date
        LOADED,     // Game is ready
        
        STARTED,    // Started (active timer)
        PAUSED,     // Paused (stopped timer)
        
        FINISHED,   // Finished (stopped timer)

        ERROR       // Any errors
    }

    public class LevelOption
    {
        public int? GameId;
        public int? Size;
    }

    public class LevelStore
    {
        private IGameGenerator gameGenerator;
        private IGameProvider gameProvider;
        private IGameResultProvider gameResultProvider;
        private IGeneratedGameResultProvider generatedGameResultProvider;

        public IGameGenerator GameGenerator { get { return gameGenerator; } }

        public LevelStore(IGameGenerator gameGenerator, IGameProvider gameProvider, IGameResultProvider gameResultProvider, IGeneratedGameResultProvider generatedGameResultProvider)
        {
            this.gameGenerator = gameGenerator;
            this.gameProvider = gameProvider;
            this.gameResultProvider = gameResultProvider;
            this.generatedGameResultProvider = generatedGameResultProvider;

            _CurrentLevelOptions = new LevelOption();
            _CurrentLevelState = LevelState.NOT_READY;
            IsCompleted = false;
            CurrentLevelOptionsChanged += OnLevelOptionsUpdated;
            GameBestDurationChanged += OnCompletedGameBestDurationUpdated;
        }

        // Best Duration

        public int? GameBestDuration {
            get { return _CurrentGame?.GameInfo?.FinishedGameDuration; }
            set {
                if (_CurrentGame?.GameInfo != null)
                {
                    _CurrentGame.GameInfo.FinishedGameDuration = value;
                }
                OnGameBestDurationChanged();
            }
        }
        
        public event Action? GameBestDurationChanged;
        private void OnGameBestDurationChanged()
        {
           GameBestDurationChanged?.Invoke();
        }

        public async void OnCompletedGameBestDurationUpdated()
        {
            if (CurrentLevelState == LevelState.FINISHED
                && IsCompleted == true
                && _CurrentGame?.GameInfo != null )
            {
                if (_CurrentGame.GameInfo.GameId != null)
                {
                    await gameResultProvider.UpdateBestDuration(_CurrentGame.GameInfo.GameId.Value, GameBestDuration);
                }
                else // Generated game
                {
                    await generatedGameResultProvider.UpdateDuration(_CurrentGame.GameInfo.Size, GameBestDuration);
                }
            }
        }

        // Level State

        private LevelState _CurrentLevelState;
        public LevelState CurrentLevelState
        {
            get { return _CurrentLevelState; }
            private set
            {
                _CurrentLevelState = value;
                OnCurrentLevelStateChanged();
            }
        }

        public event Action? CurrentLevelStateChanged;
        private void OnCurrentLevelStateChanged()
        {
            CurrentLevelStateChanged?.Invoke();
        }

        // Complete status

        public bool IsCompleted { get; private set; }

        // Level Options (pre-loadded info)
        
        private LevelOption _CurrentLevelOptions;
        public int? GameId => _CurrentLevelOptions?.GameId;
        public int? Size => _CurrentLevelOptions?.Size;
        public LevelOption CurrentLevelOption 
        {
            set 
            {
                if (value != _CurrentLevelOptions
                    || value?.GameId != _CurrentLevelOptions.GameId
                    || value?.Size != _CurrentLevelOptions.Size)
                {
                    _CurrentLevelOptions.GameId = value?.GameId;
                    _CurrentLevelOptions.Size = value?.Size;
                    OnCurrentLevelOptionsChanged();
                }
            }
        }
        
        public event Action? CurrentLevelOptionsChanged;
        private void OnCurrentLevelOptionsChanged()
        {
            CurrentLevelOptionsChanged?.Invoke();
        }
        private async void OnLevelOptionsUpdated()
        {
            IsCompleted = false;

            if (GameId == null
                && Size == null)
            {
                _CurrentGame = null;
                CurrentLevelState = LevelState.NOT_READY;
            }
            else
            {
                Game.Models.Game? game = null;
                if (GameId != null
                    && GameId != -1
                    && gameProvider != null)
                {
                    CurrentLevelState = LevelState.LOADING;
                    game = await gameProvider.GetGameLevelAsync(GameId.Value);
                }
                else if (Size != null
                    && Size != -1
                    && gameGenerator != null
                    && gameGenerator.CanGenerateGameLevel(Size ?? -1))
                {
                    CurrentLevelState = LevelState.LOADING;
                    game = await gameGenerator.GenerateGameLevelAsync(Size.Value);
                    game.GameInfo.FinishedGameDuration = await generatedGameResultProvider.GetBestDuration(Size.Value);
                }

                if (game != null)
                {
                    _CurrentLevelOptions.GameId = game.GameInfo.GameId;
                    _CurrentLevelOptions.Size = game.GameInfo.Size;
                    _CurrentGame = game;

                    CurrentLevelState = LevelState.LOADED;
                }
                else
                {
                    _CurrentGame = null;
                    CurrentLevelState = LevelState.ERROR;
                }
            }

            OnGameBestDurationChanged();
        }


        // Game (loaded info)

        private Game.Models.Game? _CurrentGame;
        public IEnumerable<Cell> CurrentGameCellField => _CurrentGame?.GameField ?? Enumerable.Empty<Cell>();

        public Color? GetGroupColor(int? groupId) 
        {
            if ( groupId != null)
            {                
                return _CurrentGame?.GetGroupColor(groupId.Value);
            }
            return null;
        }

        public void ChangeCellSate(int row, int column, CellState newCellState)
        {
            _CurrentGame?.ChangeCellState(row, column, newCellState);
            
            if (_CurrentGame != null
                && CurrentLevelState == LevelState.STARTED)
            {
                if (_CurrentGame.IsSuccessfulField)
                {
                    IsCompleted = true;
                    CurrentLevelState = LevelState.FINISHED;
                }
                else 
                {
                    IsCompleted = false;
                }
            }
        }



        public void StartGame()
        {
            if (CurrentLevelState == LevelState.LOADED)
            {
                CurrentLevelState = LevelState.STARTED;
            }
        }

        public void PauseGame()
        {
            if (CurrentLevelState == LevelState.STARTED)
            {
                CurrentLevelState = LevelState.PAUSED;
            }
        }

        public void ResumeGame()
        {
            if (CurrentLevelState == LevelState.PAUSED)
            {
                CurrentLevelState = LevelState.STARTED;
            }
        }

        public void FinishGame()
        {
            if (CurrentLevelState == LevelState.STARTED)
            {
                CurrentLevelState = LevelState.FINISHED;
            }
        }


        public void ClearGame() 
        { 
            CurrentLevelOption = new LevelOption();
        }

        public void ResetGame() 
        {
            ResetCellField();
            CurrentLevelState = LevelState.LOADED;
        }
        public void ResetCellField() 
        {
            _CurrentGame?.SetGameFieldToDefault();
        }
    }
}
