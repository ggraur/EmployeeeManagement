using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeeManagement.Models;
using EmployeeeManagement.Security;
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
using Google.Apis.Auth.OAuth2;

using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
//using Google.Apis.Drive.v2;


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

                options.SignIn.RequireConfirmedEmail = true;

                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders()
              .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");
              ;

            services.Configure<DataProtectionTokenProviderOptions>(o =>
                            o.TokenLifespan = TimeSpan.FromHours(5));

            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
                        o.TokenLifespan=TimeSpan.FromDays(3));/*video 119*/

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
            }).AddXmlSerializerFormatters();

            services.AddAuthentication()
               
               .AddGoogle(options =>
              {
                //   IConfigurationSection googleAuthNSection =
                //                    Configuration.GetSection("Authentication:Google");
                  options.ClientId = "117879105443-118r56tho9d02cm4tlesnuqloqrb0sep.apps.googleusercontent.com";
                  options.ClientSecret = "mvFRKMLTbDhuXBAKNthXvDzV";
                  //options.CallbackPath = "";
              })
               .AddFacebook(options =>
               {
                   //   IConfigurationSection googleAuthNSection =
                   //                    Configuration.GetSection("Authentication:Google");
                   options.AppId = "589686828196035";
                   options.AppSecret= "01ac752eed5d082d3ec148466b21daf4";
                   //options.CallbackPath = "";
               })

               ;


       

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            //});

            //services.AddMvc(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //            .RequireAuthenticatedUser()
            //            .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //}).AddXmlDataContractSerializerFormatters();

            /* video 99 */
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role"));

                //options.AddPolicy("DeleteRolePolicy",
                //    policy => policy.RequireAssertion(context =>
                //    context.User.IsInRole("Admin") &&
                //    context.User.HasClaim(claim => claim.Type == "Delete Role" && claim.Value == "true") ||
                //    context.User.IsInRole("Super Admin")
                //    ));

                //options.AddPolicy("CreateRolePolicy",
                //    policy => policy.RequireClaim("Create Role"));

                //options.AddPolicy("EditRolePolicy",
                //    policy => policy.RequireClaim("Edit Role", "true")
                //                    .RequireRole("Admin")
                //                    .RequireRole("Super Admin"));

                options.AddPolicy("EditRolePolicy",
                                    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()
                                    ));

                //options.AddPolicy("EditRolePolicy",
                //    policy => policy.RequireAssertion(context =>
                //    context.User.IsInRole("Admin") &&
                //    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                //    context.User.IsInRole("Super Admin")
                //    ));

                //options.AddPolicy("AdminRolePolicy",
                //   policy => policy.RequireRole("Admin"));

                options.AddPolicy("AdminRolePolicy",
               policy => policy.RequireAssertion(context =>
               context.User.IsInRole("Admin") ||
               context.User.IsInRole("Super Admin")
                ));


            });


            //getting loop and getiing erro 415
            //*******


            //services.AddSingleton<IEmployeeRepository,MockEmployeeRepository>();
            //services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();

            //            services.AddTransient<IEmployeeRepository, MockEmployeeRepository>();//memory collection

            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();//sql data base 
            
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            services.AddSingleton<DataProtectionPurposeStrings>();


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
