using FHICORC.Configuration;
using FHICORC.Core.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FHICORC.Data
{
    public class GenericRepository<T> : IRepository<T> where T : IDatabaseEntity, new()
    {
        public static Lazy<SQLiteAsyncConnection> DatabaseConnection;

        public GenericRepository() => DatabaseConnection = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags));

        public async Task<int> AddOrUpdateEntitiesAsync(IEnumerable<T> entities)
        {
            var count = 0;
            var groups = entities.GroupBy(x => x.Id.Equals(0));

            if (groups.First().Any())
                count += await DatabaseConnection.Value.InsertAllAsync(groups.First());
            if (groups.Skip(1).Any())
                count += await DatabaseConnection.Value.UpdateAllAsync(groups.Skip(1));

            return count;
        }

        public Task<int> AddOrUpdateEntityAsync(T entity) => entity.Id.Equals(0) ? DatabaseConnection.Value.InsertAsync(entity) : DatabaseConnection.Value.UpdateAsync(entity);
        public async Task<IEnumerable<T>> GetEntitiesAsync() => await DatabaseConnection.Value.Table<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetEntitiesAsync(Expression<Func<T, bool>> expression) => await DatabaseConnection.Value.Table<T>().Where(expression).ToListAsync();

        public Task<T> GetEntityAsync(int id) => DatabaseConnection.Value.FindAsync<T>(id);

        public Task<int> DeleteEntitiesAsync(Expression<Func<T, bool>> expression) => DatabaseConnection.Value.Table<T>().Where(expression).DeleteAsync();

        public Task<int> DeleteEntityAsync(T entity) => DatabaseConnection.Value.DeleteAsync(entity);

        public Task<int> DropEntityTableAsync() => DatabaseConnection.Value.DropTableAsync<T>();

        public Task<CreateTableResult> CreateEntityTableAsync() => DatabaseConnection.Value.CreateTableAsync<T>();
    }
}
