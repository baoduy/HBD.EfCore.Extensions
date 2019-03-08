using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace HBD.EfCore.EntityResolver.Tests.Helpers
{
    public static class SqliteMemory
    {
        #region Public Properties

        public static LoggerFactory DebugLoggerFactory =>
             new LoggerFactory(new[]
            {
               new DebugLoggerProvider()
            }, new LoggerFilterOptions
            {
                Rules =
                {
                    new LoggerFilterRule("EfCoreDebugger", string.Empty,
                        LogLevel.Trace, (m, n, l) => m.Contains("Query"))
                }
            });

        #endregion Public Properties

        #region Public Methods

        public static DbContextOptionsBuilder UseSqliteMemory(this DbContextOptionsBuilder @this)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var sqliteConnection = new SqliteConnection(connectionStringBuilder.ToString());
            sqliteConnection.Open();

            @this.UseSqlite(sqliteConnection);
            return @this;
        }

        #endregion Public Methods
    }
}