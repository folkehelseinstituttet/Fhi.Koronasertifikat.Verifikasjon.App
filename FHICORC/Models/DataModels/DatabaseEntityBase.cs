using FHICORC.Core.Data;
using SQLite;

namespace FHICORC.Models.DataModels
{
    public class DatabaseEntityBase : IDatabaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
