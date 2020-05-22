using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace Unit_of_work
{
  public static  class MyDependencies
    {
         public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext :DbContext
        {
            services.AddScoped<IGetRepositoryFactory, UnitofWork<TContext>>();
            services.AddScoped<IUnitofWork,UnitofWork<TContext> >();
            services.AddScoped<IUnitofWork<TContext>, UnitofWork<TContext>>();
            return services;

        }
    }
}
