using SQLite;

namespace FHICORC.Models
{
    public class Batches
    {
        [PrimaryKey]
        public string BatchID { get; set; }

        public string Expires { get; set; }

        public int Deleted { get; set; }
    }
}
