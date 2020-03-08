using SQLite;

//get a connection
namespace CardinalScout2019
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}