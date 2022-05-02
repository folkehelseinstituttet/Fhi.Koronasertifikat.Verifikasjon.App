using FHICORC.Core.Data;
using FHICORC.Core.Services.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FHICORC.Data
{
    public class GenericRepositoryProxy<T> : IRepository<T> where T : IDatabaseEntity, new()
    {
        private readonly AsyncLazy<GenericRepository<T>> _instance;

        public GenericRepositoryProxy() => _instance = new AsyncLazy<GenericRepository<T>>(async () =>
        {
            var instance = new GenericRepository<T>();
            _ = await GenericRepository<T>.DatabaseConnection.Value.CreateTableAsync<T>();
            return instance;
        });

        public Task<int> AddOrUpdateEntitiesAsync(IEnumerable<T> entities) => Task.Run(async () => (await _instance.Value.ConfigureAwait(false)).AddOrUpdateEntitiesAsync(entities)).Unwrap();
        public Task<int> AddOrUpdateEntityAsync(T entity) => Task.Run(async () => (await _instance.Value.ConfigureAwait(false)).AddOrUpdateEntityAsync(entity)).Unwrap();
        public Task<int> DeleteEntitiesAsync(Expression<Func<T, bool>> expression) => Task.Run(async () => (await _instance.Value.ConfigureAwait(false)).DeleteEntitiesAsync(expression)).Unwrap();
        public Task<int> DeleteEntityAsync(T entity) => Task.Run(async () => (await _instance.Value.ConfigureAwait(false)).DeleteEntityAsync(entity)).Unwrap();
        public Task<IEnumerable<T>> GetEntitiesAsync() => Task.Run(async () => (await _instance.Value.ConfigureAwait(false)).GetEntitiesAsync()).Unwrap();
        public Task<IEnumerable<T>> GetEntitiesAsync(Expression<Func<T, bool>> expression) => Task.Run(async () => (await _instance.Value.ConfigureAwait(false)).GetEntitiesAsync(expression)).Unwrap();
        public Task<T> GetEntityAsync(int id) => Task.Run(async () => (await _instance.Value.ConfigureAwait(false)).GetEntityAsync(id)).Unwrap();
        public Task<int> DropEntityTableAsync() => Task.Run(async () => (await _instance.Value.ConfigureAwait(false)).DropEntityTableAsync()).Unwrap();
        public Task<CreateTableResult> CreateEntityTableAsync() => Task.Run(async () => (await _instance.Value.ConfigureAwait(false)).CreateEntityTableAsync()).Unwrap();
    }
}
