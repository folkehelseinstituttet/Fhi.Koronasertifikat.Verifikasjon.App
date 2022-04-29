using SQLite;

namespace FHICORC.Services.Interfaces
{
    public interface ISqlConnection
    {
        SQLiteAsyncConnection GetConnection();
    }
}
