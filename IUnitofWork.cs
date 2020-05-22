using Microsoft.EntityFrameworkCore;
using System;

namespace Unit_of_work
{
 public interface IUnitofWork:IDisposable
    {
        public IRepositoryAsync<T> repositoryAsync<T>() where T : class;
        void CommitChanges();

    }

    public interface IUnitofWork<TContext> : IUnitofWork where TContext: DbContext
    {

        TContext Context { get; }
    }

}
