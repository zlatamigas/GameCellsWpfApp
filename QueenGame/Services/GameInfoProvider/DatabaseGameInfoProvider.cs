using Microsoft.EntityFrameworkCore;
using QueenGame.DB;
using QueenGame.DB.DbContexts;
using QueenGame.DB.DTOs;
using QueenGame.Game.Models;

namespace QueenGame.Services.GameInfoProvider
{
    public class DatabaseGameInfoProvider : IGameInfoProvider
    {
        private readonly AppDbContextFactory dbContextFactory;

        public DatabaseGameInfoProvider(AppDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<GameInfo?> GetGameInfo(int gameId)
        {
            using (var db = dbContextFactory.CreateDbContext())
            {
                DbGame? dbGameLevel = await db.Games
                    .Where(gl => gl.GameId == gameId)
                    .FirstOrDefaultAsync();

                return dbGameLevel != null ? DbParser.ParseGameLevelInfo(dbGameLevel) : null;
            }
        }

        public async Task<IEnumerable<GameInfo>> GetGameInfoAll()
        {
            using (var db = dbContextFactory.CreateDbContext())
            {
                IEnumerable<DbGame> dbGameLevels = await db.Games.ToListAsync();

                return dbGameLevels.Select(DbParser.ParseGameLevelInfo);
            }
        }
    }
}
