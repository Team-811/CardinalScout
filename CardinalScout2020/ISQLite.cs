using SQLite;

namespace CardinalScout2020
{
    /// <summary>
    /// Interface for the SQLite Database
    /// </summary>
    public interface ISQLite
    {
        /// <summary>
        /// Make sure the database has a method which gets a new SQLite database connection.
        /// </summary>
        /// <returns>New connection to the database.</returns>
        SQLiteConnection GetConnection();
    }
}