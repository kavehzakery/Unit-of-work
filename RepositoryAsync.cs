using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Unit_of_work
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        protected readonly DbContext _dBContext;
        protected readonly DbSet<T> _Dbset;
        public RepositoryAsync(DbContext dBContext)
        {
            _dBContext = dBContext;
            _Dbset = _dBContext.Set<T>();
        }

        public Task AddParams(params T[] paramEntity)
        {
            throw new NotImplementedException();
        }

        public async Task BulkAddAsync(IEnumerable<T> listEntity)
        {
            await _Dbset.AddRangeAsync(listEntity);
        }

        public async Task Delete(int id)
        {
            var result = await FindbyID(id);
            Delete(result);
        }

        public void Delete(T t)
        {
            //_context.Entry(t).State = EntityState.Deleted;
            _Dbset.Remove(t);
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> where = null)
        {
            var result = await _Dbset.AnyAsync(where);
            return result;
        }

        public async Task<T> FindAny(Expression<Func<T, bool>> where = null)
        {
            var reult = await _Dbset.FirstOrDefaultAsync(where);
            return reult;
        }

        public async Task<T> FindbyID(int id)
        {
            var result = await _Dbset.FindAsync(id);
            return result;
        }

        public IQueryable<T> fullQuary(Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includePoperties = null, int? page = 1, int? pageItem = 10)
        {
            IQueryable<T> quary = _Dbset;
            if (where != null)
            {
                quary = _Dbset.Where(where);
            }
            if (orderBy != null)
            {
                quary = orderBy(quary);
            }
            if (!string.IsNullOrEmpty(includePoperties))
            {
                var items = includePoperties.Split(',');
                foreach (var item in items)
                {
                    quary = quary.Include(item);
                }
            }
            if (page != null)
            {
                var skip = (page.Value - 1) * pageItem.Value;
                quary = quary.Skip(skip).Take(pageItem.Value);
            }

            return quary;
        }

        public async Task SingleAddAsync(T entity)
        {
            await _Dbset.AddAsync(entity);
           

        }
    }
}
