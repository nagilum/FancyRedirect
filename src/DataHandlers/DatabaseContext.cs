using Microsoft.EntityFrameworkCore;

namespace FancyRedirect.DataHandlers
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={StorageHandler.StoragePath}");
        }

        #region DbSets

        public DbSet<UrlEntry> UrlEntries { get; set; }

        #endregion
    }
}
