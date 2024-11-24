using Microsoft.EntityFrameworkCore;
using QueenGame.DB;
using QueenGame.DB.DbContexts;
using QueenGame.DB.DTOs;
using QueenGame.Game.Models;

namespace QueenGame.Services.GameProvider
{
    public class DatabaseGameProvider : IGameProvider
    {
        private readonly AppDbContextFactory dbContextFactory;

        public DatabaseGameProvider(AppDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<Game.Models.Game?> GetGameLevelAsync(int gameId)
        {
            using (var db = dbContextFactory.CreateDbContext()) 
            {
                DbGame? dbGameLevel = await db.Games
                    .Where(gl => gl.GameId == gameId)
                    .FirstOrDefaultAsync();

                return dbGameLevel != null ? DbParser.ParseGameLevel(dbGameLevel) : null;
            }
        }
    }
}
