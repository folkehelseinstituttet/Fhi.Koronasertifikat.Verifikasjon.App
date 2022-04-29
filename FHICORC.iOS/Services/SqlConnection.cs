using System;
using System.IO;
using FHICORC.iOS.Services;
using FHICORC.Services.Interfaces;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqlConnection))]
namespace FHICORC.iOS.Services
{
    public class SqlConnection : ISqlConnection
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "Revocations.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}
