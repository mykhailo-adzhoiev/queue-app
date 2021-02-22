using System.Collections.Generic;
using System.Threading.Tasks;

namespace QueueMessageApp.DAL.Repository
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        IEnumerable<TEntity> GetAll();
        /// <summary>
        /// Creates entity row in database
        /// </summary>
        Task<TEntity> AddAsync(TEntity entity);
        /// <summary>
        /// Updates entity row in database
        /// </summary>
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
