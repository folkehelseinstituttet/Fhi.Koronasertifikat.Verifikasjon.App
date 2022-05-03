using SQLite;
using System;

namespace FHICORC.Core.Data.Models
{
    public class AppLastFetchingDates : DatabaseEntityBase
    {
        [Indexed, NotNull, MaxLength(12)]
        public string Name { get; set; }

        [NotNull]
        public DateTime? LastFetch { get; set; }
    }
}
