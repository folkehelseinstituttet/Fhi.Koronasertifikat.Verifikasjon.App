using System;
using System.Collections.Generic;
using System.Text;

namespace FHICORC.Core.Data
{
    public interface IDatabaseEntity
    {
        /// <summary>
        /// The primary key of the entity.
        /// <para>
        /// Must implement <see cref="SQLite.PrimaryKeyAttribute"/>
        /// </para>
        /// </summary>
        int Id { get; set; }
    }
}
