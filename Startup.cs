using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeeManagement.Models;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmployeeeManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc(option => option.EnableEndpointRouting = false);
            //services.AddMvc().AddXmlDataContractSerializerFormatters();

            //*******
            //getting loop and getiing erro 415

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlDataContractSerializerFormatters();


            //getting loop and getiing erro 415
            //*******


            //services.AddSingleton<IEmployeeRepository,MockEmployeeRepository>();
            //services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();

            //            services.AddTransient<IEmployeeRepository, MockEmployeeRepository>();//memory collection
            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();//sql data base 

        }



        //public void ConfigureServices(IServiceCollection services)
        //{

        //    services.AddDbContextPool<AppDbContext>(
        //        options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));


        //    services.AddIdentity<IdentityUser,IdentityRole>(options =>
        //    {
        //        options.Password.RequiredLength = 10;
        //        options.Password.RequiredUniqueChars = 3;
        //    }).AddEntityFrameworkStores<AppDbContext>();




        //   services.AddMvc(option => option.EnableEndpointRouting = false);
        //    services.AddMvc().AddXmlDataContractSerializerFormatters();

        //    //services.AddMvc(options => {
        //    //    var policy = new AuthorizationPolicyBuilder()
        //    //            .RequireAuthenticatedUser()
        //    //            .Build();
        //    //    options.Filters.Add(new AuthorizeFilter(policy));
        //    //}).AddXmlDataContractSerializerFormatters();

        //    //services.AddSingleton<IEmployeeRepository,MockEmployeeRepository>();
        //    //services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();

        //    //            services.AddTransient<IEmployeeRepository, MockEmployeeRepository>();//memory collection
        //    services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();//sql data base 

        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
             if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                //app.UseStatusCodePages();
                //
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
                        
            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
