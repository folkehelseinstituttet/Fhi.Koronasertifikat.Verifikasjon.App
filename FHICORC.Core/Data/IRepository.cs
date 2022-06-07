using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FHICORC.Core.Data
{
    public interface IRepository<T> where T : IDatabaseEntity
    {
        /// <summary>
        /// Quries the database for all results
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<T>> GetEntitiesAsync();
        /// <summary>
        /// Queries the database for results satisfying the predicate expression.
        /// </summary>
        /// <param name="expression">Predicate to filter on.</param>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        Task<IEnumerable<T>> GetEntitiesAsync(Expression<Func<T, bool>> expression);
        /// <summary>
        ///  Attempts to retrieve an object with the given <strong>primary key</strong> from the table associated with the specified type <typeparamref name="T"/>
        /// <param name="primaryKey">The primary key.</param>
        /// </summary>
        /// <returns>The object of type <typeparamref name="T"/> with the given primary key or null if the object is not found.</returns>
        Task<T> GetEntityAsync(int primaryKey);
        /// <summary>
        /// Adds or updates all of the columns of the <strong>Table</strong>&lt;<typeparamref name="T"/>&gt;, for all specified entities&lt;<typeparamref name="T"/>&gt;. 
        /// If the entity's <strong>primary key</strong> is <strong>0</strong>, 
        /// the entity is added, else the entry in the <strong>Table</strong>&lt;<typeparamref name="T"/>&gt; matching the <strong>primary key</strong> is updated except for its <strong>primary key</strong> column.
        /// </summary>
        /// <param name="entities">Entities to be added or updated.</param>
        /// <returns>The number of rows either added or updated in <strong>Table</strong>&lt;<typeparamref name="T"/>&gt;.</returns>
        Task<int> AddOrUpdateEntitiesAsync(IEnumerable<T> entities);
        /// <summary>
        /// If the <strong>primary key</strong> of the specified entity&lt;<typeparamref name="T"/>&gt; is <strong>0</strong>, the entity&lt;<typeparamref name="T"/>&gt; is added. 
        /// Else the entity&lt;<typeparamref name="T"/>&gt; in the <strong>Table</strong>&lt;<typeparamref name="T"/>&gt; matching the <strong>primary key</strong> is updated except for its <strong>primary key</strong> column.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns><strong>1</strong> if entity&lt;<typeparamref name="T"/>&gt; was added else <strong>0</strong>.</returns>
        Task<int> AddOrUpdateEntityAsync(T entity);
        /// <summary>
        /// Delete all the rows that match the specified expression.
        /// </summary>
        /// <param name="expression">The predicate query.</param>
        /// <returns>The number of rows deleted.</returns>
        Task<int> DeleteEntitiesAsync(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Delete the row matching the <strong>primary key</strong> of the specified entity&lt;<typeparamref name="T"/>&gt;.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns><strong>1</strong> if entity&lt;<typeparamref name="T"/>&gt; was deleted else <strong>0</strong>.</returns>
        Task<int> DeleteEntityAsync(T entity);
        /// <summary>
        /// Drops <strong>Table</strong>&lt;<typeparamref name="T"/>&gt; in the database. This is action is irreversible.
        /// </summary>
        /// <returns></returns>
        Task<int> DropEntityTableAsync();
        /// <summary>
        /// Creates <strong>Table</strong>&lt;<typeparamref name="T"/>&gt; in the database. 
        /// As well as any specified indexes on the columns of the <strong>Table</strong>&lt;<typeparamref name="T"/>&gt;.
        /// </summary>
        /// <returns><see cref="CreateTableResult.Created"/> if the table was created else <see cref="CreateTableResult.Migrated"/>.</returns>
        Task<CreateTableResult> CreateEntityTableAsync();
    }
}
