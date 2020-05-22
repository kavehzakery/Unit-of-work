using System;
using System.Collections.Generic;
using System.Text;

namespace Unit_of_work
{
    public interface IGetRepositoryFactory
    {
        public IRepositoryAsync<T> repositoryAsync<T>() where T : class;
    }
}
