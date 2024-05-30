using IdentityDb.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityDb.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void EditAsync(T entity)
        {
            _dbSet.Attach(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> InsertAsync(T entity)
        {
            var response = await _dbSet.AddAsync(entity);
            return response.Entity;
        }

        public void RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void StateDetach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void StateModify(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
