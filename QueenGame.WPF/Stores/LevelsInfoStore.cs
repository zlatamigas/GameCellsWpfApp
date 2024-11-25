using QueenGame.Game.GameGenerator;
using QueenGame.Game.Models;
using QueenGame.Services.GameInfoProvider;
using QueenGame.Services.GameProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.WPF.Stores
{
    public class LevelsInfoStore
    {
        private readonly IGameInfoProvider gameInfoProvider;

        private List<GameInfo> GameInfos;

        public LevelsInfoStore(IGameInfoProvider gameInfoProvider)
        {
            this.gameInfoProvider = gameInfoProvider;
        }

        public async Task Load() 
        {
            GameInfos = (await gameInfoProvider.GetGameInfoAll()).ToList();
        }

        public IEnumerable<GameInfo> GetGameInfos() 
        { 
            return GameInfos;
        }

        public void UpdateLevelOptionDuration(int currentGameId, int? duration)
        {
            GameInfo? currentGame = GameInfos.FirstOrDefault(v => v.GameId == currentGameId);
            if (currentGame != null)
            {
                currentGame.FinishedGameDuration = duration;
            }
        }

        public bool HasNextLevelOption(int currentGameId)
        {
            return GetNextLevelOption(currentGameId) != null;
        }

        public LevelOption? GetNextLevelOption(int currentGameId) 
        {
            GameInfo? nextGameInfo = null;
            GameInfo? currentGame = GameInfos.FirstOrDefault(v => v.GameId == currentGameId);
            if (currentGame != null) 
            {
                int currentGameIndex = GameInfos.IndexOf(currentGame);
                int index = currentGameIndex + 1;

                while (index != currentGameIndex && nextGameInfo == null)
                {
                    if (index == GameInfos.Count) 
                    {
                        index = 0;
                        if (index == currentGameIndex) 
                        { 
                            break;
                        }
                    }

                    if (GameInfos.ElementAt(index).FinishedGameDuration == null)
                    {
                        nextGameInfo = GameInfos.ElementAt(index);
                    }
                    else 
                    { 
                        index++;
                    }
                }
            }

            return nextGameInfo != null 
                ? new LevelOption {
                        GameId = nextGameInfo.GameId,
                        Size = null,
                    }
                : null;
        }   
    }
}
