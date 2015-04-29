using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    interface IRepository<T> where T : class
    {
        Task<int> Add(T t);
        Task<int> AddCollection(ICollection<T> t);
        Task<T> Update(T updated, int key);
        Task<int> Delete(T t);
        Task<int> Count();
        Task<ICollection<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Find(Expression<Func<T, bool>> expression);
        Task<ICollection<T>> FindAll(Expression<Func<T, bool>> expression);
        Task<T> FindWithInclude(Expression<Func<T, object>> include, Expression<Func<T, bool>> expression);
        Task<ICollection<T>> FindAllWithInclude(Expression<Func<T, object>> include, Expression<Func<T, bool>> expression);

    }
}
