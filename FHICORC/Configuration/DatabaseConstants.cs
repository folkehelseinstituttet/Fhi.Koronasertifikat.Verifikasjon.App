using SQLite;
using System;
using System.IO;

namespace FHICORC.Configuration
{
    public static class DatabaseConstants
    {
        public const string DatabaseFilename = "SQLiteDatabase";
        public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}
