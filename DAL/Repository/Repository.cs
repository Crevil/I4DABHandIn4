using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly Context _context;

        public Repository(Context context)
        {
            _context = context;
        }

        public async Task<int> Add(T t)
        {
            _context.Set<T>().Add(t);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddCollection(ICollection<T> t)
        {
            using (var context = new Context())
            {
                context.Set<T>().AddRange(t);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<T> Update(T updated, int key)
        {
            if (updated == null)
                return null;

            var old = await _context.Set<T>().FindAsync(key);

            if (old != null)
            {
                _context.Entry(old).CurrentValues.SetValues(updated);
                await _context.SaveChangesAsync();
            }
            return old;
        }

        public async Task<int> Delete(T t)
        {
            _context.Set<T>().Remove(t);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Count()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<ICollection<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Find(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(expression);
        }

        public async Task<ICollection<T>> FindAll(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<ICollection<T>> FindAllDoubleWhere(Expression<Func<T, bool>> exp1, Expression<Func<T, bool>> exp2)
        {
            return await _context.Set<T>().Where(exp1).Where(exp2).ToListAsync();
        }

        public async Task<T> FindWithInclude(Expression<Func<T, object>> include, Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Include(include).SingleOrDefaultAsync(expression);
        }

        public async Task<ICollection<T>> FindAllWithInclude(Expression<Func<T, object>> include, Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Include(include).Where(expression).ToListAsync();
        }
    }
}
