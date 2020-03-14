using SQLite;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(CardinalScout2020.SQLite_android))]

namespace CardinalScout2020
{
    /// <summary>
    /// This class creates a new connection to the SQLite database. The file for the database is stored on the device at the unique generated path name.
    /// </summary>
    public class SQLite_android: ISQLite
    {        
        /// <summary>
        /// Return a new connection to the database on the device.
        /// </summary>
        /// <returns>A new connection to the database.</returns>
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "scoutdb20.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            var conn = new SQLiteConnection(path);

            return conn;
        }

        /// <summary>
        /// Returns the unique path to the database based on the device.
        /// </summary>
        /// <returns>Path to the database on the device</returns>
        public static string GetDatabasePath()
        {
            var sqliteFilename = "scoutdb20.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            return path;
        }      
    }
}