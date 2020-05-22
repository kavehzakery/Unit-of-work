using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Unit_of_work
{
   public interface IRepositoryAsync<T> where T:class
    {
    
        Task SingleAddAsync(T entity);
        Task BulkAddAsync(IEnumerable<T> listEntity);
        Task AddParams(params T[] paramEntity);
        Task Delete(int id);
        void Delete(T t);
        Task<bool> ExistAsync(Expression<Func<T, bool>> where = null);
        Task<T> FindAny(Expression<Func<T, bool>> where = null);
        Task<T> FindbyID(int id);
        IQueryable<T> fullQuary(Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            , string includePoperties = null
            , int? page = 1
            , int? pageItem = 10);

    }
}
