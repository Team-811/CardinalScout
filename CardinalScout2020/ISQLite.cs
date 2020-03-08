using SQLite;

//get a connection
namespace CardinalScout2020
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}