
using Microsoft.EntityFrameworkCore;
using UnitOfWorkTrial.Models;

namespace UnitOfWorkTrial.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            //we are initializing the context object and DbSet variable
            _context = context;
            //Whatever Entity name we specify while creating the instance of GenericRepository
            //That Entity name  will be stored in the DbSet<T> variable
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public async Task InsertAsync(T Entity)
        {
            await _dbSet.AddAsync(Entity);
        }

        public async Task UpdateAsync(T Entity)
        {
            _dbSet.Update(Entity);
        }

        public async Task DeleteAsync(object Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            if(entity != null)
            {
               _dbSet.Remove(entity);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }  
}
