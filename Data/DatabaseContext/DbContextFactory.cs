using atf.Core.Config;
using Microsoft.EntityFrameworkCore;


namespace atf.Data.DatabaseContext
{
    /// <summary>
    /// Provides factory methods for creating instances of <see cref="TestDbContext"/>.
    /// Supports SQL Server and in-memory database providers.
    /// </summary>
    public static class DbContextFactory
    {
        private static string _connectionString = ConfigManager.Get<string>("ConnectionStrings:DefaultConnection") 
                                                  ?? "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=true;";

        public static TestDbContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);
            return new TestDbContext(optionsBuilder.Options);
        }

        public static TestDbContext CreateInMemoryContext(string databaseName = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString());
            return new TestDbContext(optionsBuilder.Options);
        }

        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static void EnsureDatabaseCreated()
        {
            try
            {
                using var context = CreateContext();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbContextFactory] EnsureDatabaseCreated failed: {ex}");
                throw;
            }
        }

        public static void CleanDatabase()
        {
            try
            {
                using var context = CreateContext();
                context.Users.RemoveRange(context.Users);
                context.Products.RemoveRange(context.Products);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbContextFactory] CleanDatabase failed: {ex}");
                throw;
            }
        }
    }
}
