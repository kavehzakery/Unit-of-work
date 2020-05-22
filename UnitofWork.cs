using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unit_of_work
{
    public class UnitofWork<TContext> : IUnitofWork<TContext>, IGetRepositoryFactory, IUnitofWork where TContext : DbContext, IDisposable
    { /// <summary>
    /// /////////////////////////////////hame az noe T context hast va Repository az noe t pas ye Factory dorost kardim ke age oon ro implement konim har chi dakheleshe ro ham implement kardim
    /// </summary>
       public TContext Context { get; }
        public UnitofWork(TContext context)
        {
           Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private Dictionary<Type, object> _repositories;
        public void CommitChanges()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public IRepositoryAsync<T> repositoryAsync<T>() where T : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(T);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryAsync<T>(Context);
            return (IRepositoryAsync<T>)_repositories[type];

            //RepositoryAsync<T> repositoryAsync = new RepositoryAsync<T>(Context);
            //return   repositoryAsync;
        }
    }
}
