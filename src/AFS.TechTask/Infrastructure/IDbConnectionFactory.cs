using System.Data;

namespace AFS.TechTask.Infrastructure
{
    /// <summary>
    /// Factory for returning a database connection.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Return an open <see cref="IDbConnection"/>.
        /// </summary>
        /// <returns>An open <see cref="IDbConnection"/>.</returns>
        Task<IDbConnection> CreateConnectionAsync(); 
    }
}
