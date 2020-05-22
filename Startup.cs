using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DomainModel;
using KavehZakeryMVC.ViewServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service;
using Unit_of_work;

namespace KavehZakeryMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region DI
            var quantcart = new CartQuantity
            {
                quantity=3
            };
            services.AddSingleton(quantcart);
            #endregion

            #region Database
            services.AddDbContext<KavehZakeryDB>(options =>
           options.UseSqlServer(
               Configuration.GetConnectionString("KavehZakeryDataBase")));
            #endregion
            #region AddScoped UnitofWork
            services.AddDbContext<KavehZakeryDB>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("KavehZakeryDataBase"), builder => builder.MigrationsAssembly(typeof(Startup).Assembly.FullName)
           )).AddUnitOfWork<KavehZakeryDB>();

            #endregion
            #region AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion
            #region Cookies
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            #endregion
            #region sessions
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                //options.Cookie.IsEssential = true;
            });

            #endregion

            services.AddControllersWithViews();
            //Install-Package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation -Version 3.1.0
            services.AddRazorPages().AddRazorRuntimeCompilation();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            loggerFactory.AddFile("Log/{Date}-MyLog.txt");


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                         name: "areas",
                         pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
