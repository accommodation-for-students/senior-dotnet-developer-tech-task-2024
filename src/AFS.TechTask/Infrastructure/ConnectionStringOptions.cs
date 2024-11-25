namespace AFS.TechTask.Infrastructure
{
    /// <summary>
    /// Configurable database connection string options
    /// </summary>
    public class ConnectionStringOptions
    {
        /// <summary>
        /// The connection string to conect to the database with.
        /// </summary>
        public string ConnectionString { get; set; } = $"Data Source=./AFS_Test.db;";
    }
}
