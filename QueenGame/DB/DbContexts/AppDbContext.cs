using Microsoft.EntityFrameworkCore;
using QueenGame.DB.DTOs;

namespace QueenGame.DB.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<DbGame> Games { get; set; }
        public DbSet<DbGeneratedGameResult> GeneratedGameResults { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) 
        { 
            Database.EnsureCreated();
        }
    }
}
