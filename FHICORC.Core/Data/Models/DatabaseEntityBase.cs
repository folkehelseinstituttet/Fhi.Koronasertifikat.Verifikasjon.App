using SQLite;

namespace FHICORC.Core.Data.Models
{
    public class DatabaseEntityBase : IDatabaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
