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

        public LevelOption GetNextLevelOption(int currentGameId) 
        {
            GameInfo? nextGameInfo = null;
            GameInfo? currentGame = GameInfos.FirstOrDefault(v => v.GameId == currentGameId);
            if (currentGame != null) 
            {
                int currentGameIndex = GameInfos.IndexOf(currentGame);

                if (currentGameIndex == GameInfos.Count - 1)
                {
                    nextGameInfo = GameInfos.ElementAt(0);
                }
                else 
                { 
                    nextGameInfo = GameInfos.ElementAt(currentGameIndex+1);
                }
            }

            return new LevelOption {
                GameId = nextGameInfo?.GameId,
                Size = null,
            };
        }   
    }
}
