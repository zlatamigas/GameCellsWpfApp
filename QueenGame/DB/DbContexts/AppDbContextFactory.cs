using Microsoft.EntityFrameworkCore;

namespace QueenGame.DB.DbContexts
{
    public class AppDbContextFactory
    {
        private readonly string CONNECTION_STRING;

        public AppDbContextFactory(string connectionStr)
        {
            CONNECTION_STRING = connectionStr;
        }

        public AppDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseSqlite(CONNECTION_STRING)
                .Options;

            return new AppDbContext(options);
        }
    }
}
