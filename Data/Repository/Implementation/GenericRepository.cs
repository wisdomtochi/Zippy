using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Zippy.Data.Context;
using Zippy.Data.Repository.Interface;

namespace Zippy.Data.Repository.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ZippyDbContext _dbContext;
        private readonly DbSet<T> table;

        public GenericRepository(ZippyDbContext dbContext)
        {
            _dbContext = dbContext;
            table = _dbContext.Set<T>(); // Ensure ZippyDbContext inherits from DbContext
        }

        public async Task AddAsync(T entity)
        {
            await table.AddAsync(entity);
        }

        public async Task AddRangeAsync(IList<T> entities)
        {
            await table.AddRangeAsync(entities);
        }

        public async Task DeleteAsync(Guid EntityId)
        {
            var entity = await ReadSingleAsync(EntityId);
            table.Remove(entity);
        }

        public void DeleteRange(IList<T> entities)
        {
            table.RemoveRange(entities);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await table.ToListAsync();
        }

        public IQueryable<T> ReadAllQuery()
        {
            return table.AsQueryable();
        }

        public async Task<T> ReadSingleAsync(Guid EntityId)
        {
            return await table.FindAsync(EntityId);
        }

        public void Update(T entity)
        {
            table.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IList<T> entities)
        {
            table.UpdateRange(entities);
        }

        public async Task<bool> SaveAsync()
        {
            bool result = false;

            try
            {
                result = await _dbContext.SaveChangesAsync() >= 0;
            }
            catch
            {
                throw;
            }

            return result;
        }
    }
}
