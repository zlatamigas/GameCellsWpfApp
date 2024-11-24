using Microsoft.EntityFrameworkCore;
using QueenGame.DB;
using QueenGame.DB.DbContexts;
using QueenGame.DB.DTOs;
using QueenGame.Game.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.Services.GeneratedGameResultProvider
{
    public class DatabaseGeneratedGameResultProvider : IGeneratedGameResultProvider
    {
        private readonly AppDbContextFactory dbContextFactory;

        public DatabaseGeneratedGameResultProvider(AppDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int?> GetBestDuration(int size)
        {
            using (var db = dbContextFactory.CreateDbContext())
            {
                DbGeneratedGameResult? dbGame = await db.GeneratedGameResults.FirstOrDefaultAsync(v => v.Size == size);
                
                return dbGame?.Duration;
            }
        }

        public async Task<IEnumerable<GeneratedGameResult>> GetGeneratedGameResults()
        {
            using (var db = dbContextFactory.CreateDbContext())
            {
                return db.GeneratedGameResults.Select(gr => DbParser.ParseGeneratedGameResult(gr)).ToList();
            }
        }

        public async Task UpdateDuration(int size, int? duration)
        {
            using (var db = dbContextFactory.CreateDbContext())
            {
                DbGeneratedGameResult? dbGame = await db.GeneratedGameResults.FirstOrDefaultAsync(v => v.Size == size);

                if (dbGame != null)
                {
                    dbGame.Duration = duration;
                }
                else 
                {
                    dbGame = new DbGeneratedGameResult { Size = size, Duration = duration };
                    await db.GeneratedGameResults.AddAsync(dbGame);
                }
                db.SaveChanges();
            }
        }
    }
}
