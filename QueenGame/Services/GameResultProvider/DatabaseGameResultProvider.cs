using Microsoft.EntityFrameworkCore;
using QueenGame.DB.DbContexts;
using QueenGame.DB.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.Services.GameResultProvider
{
    public class DatabaseGameResultProvider : IGameResultProvider
    {
        private readonly AppDbContextFactory dbContextFactory;

        public DatabaseGameResultProvider(AppDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int?> GetBestDuration(int gameId)
        {
            using (var db = dbContextFactory.CreateDbContext())
            {
                DbGame? dbGame = await db.Games.FirstOrDefaultAsync(v => v.GameId == gameId);
                
                return dbGame?.FinishedGameDuration;
            }
        }

        public async Task<bool> UpdateBestDuration(int gameId, int? duration)
        {
            using (var db = dbContextFactory.CreateDbContext())
            {
                DbGame? dbGame = await db.Games.FirstOrDefaultAsync(v => v.GameId == gameId);

                if (dbGame != null)
                {
                    dbGame.FinishedGameDuration = duration;
                    db.SaveChanges();

                    return true;
                }
                else 
                { 
                    return false;
                }

            }
        }
    }
}
