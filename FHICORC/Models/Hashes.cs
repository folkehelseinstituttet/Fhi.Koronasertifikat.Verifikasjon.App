using System;
using SQLite;

namespace FHICORC.Models
{
    public class Hashes
    {
        [PrimaryKey, AutoIncrement]
        public string HashID { get; set; }

        public string BatchID { get; set; }

        public string Hash { get; set; }

        public string Kid { get; set; }

        public string HashType { get; set; }
    }
}
