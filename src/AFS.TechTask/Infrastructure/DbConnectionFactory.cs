using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.Common;

namespace AFS.TechTask.Infrastructure
{
    /// <summary>
    /// Factory for returning a database connection.
    /// </summary>
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString;

        private Lazy<Task<bool>> initialised;

        public DbConnectionFactory(IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            this.connectionString = connectionStringOptions.Value.ConnectionString;
            this.initialised = new Lazy<Task<bool>>(Initialise());
        }

        /// <summary>
        /// Return an open <see cref="SqlConnection"/>.
        /// </summary>
        /// <returns>An open <see cref="SqlConnection"/>.</returns>
        public async Task<IDbConnection> CreateConnectionAsync()
        {
            await this.initialised.Value;

            IDbConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            return connection;
        }

        private async Task<bool> Initialise()
        {
            const string sql = @"PRAGMA foreign_keys = ON;

                CREATE TABLE IF NOT EXISTS Property (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PropertyType INTEGER NOT NULL,
                    Country TEXT NOT NULL,
                    IngestRunId DATETIME
                );

                CREATE TABLE IF NOT EXISTS Bedroom (
                    BedroomId INTEGER PRIMARY KEY AUTOINCREMENT,
                    PropertyId INTEGER NOT NULL,
                    Available INTEGER NOT NULL,
                    RoomSize TEXT NOT NULL,
                    BedSize TEXT NOT NULL,
                    Rent INTEGER NOT NULL,
                    Deposit INTEGER NOT NULL,
                    FOREIGN KEY (PropertyId) REFERENCES Property(Id)
                );

                CREATE TABLE IF NOT EXISTS Photo (
                    PhotoId INTEGER PRIMARY KEY AUTOINCREMENT,
                    PropertyId INTEGER NOT NULL,
                    Uri TEXT NOT NULL,
                    FOREIGN KEY (PropertyId) REFERENCES Property(Id),
                    CONSTRAINT UC_PhotoUri UNIQUE (PropertyId, Uri)
                );";

            using (DbConnection connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql);
            };

            return true;
        }
    }
}
