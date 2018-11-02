using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    public static class SqliteMemory
    {
        public static DbContextOptionsBuilder UseSqliteMemory(this DbContextOptionsBuilder @this)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var sqliteConnection = new SqliteConnection(connectionStringBuilder.ToString());
            sqliteConnection.Open();

            @this.UseSqlite(sqliteConnection);
            return @this;
        }

        public static DbContext LogToConsole(this DbContext context)
        {
            var contextServices = ((IInfrastructure<IServiceProvider>)context).Instance;
            var loggerFactory = contextServices.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddDebug((m,l)=> m.Contains("Query"));
            return context;
        }
    }
}
