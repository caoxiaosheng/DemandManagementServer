using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemandManagementServer.DAL;
using DemandManagementServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DemandManagementServer
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
            MapperInitialization.Initialize();

            services.AddDbContext<DemandDbContext>(option=>option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISoftwareVersionService, SoftwareVersionService>();
            services.AddScoped<IDemandService, DemandService>();

            //services.AddMvc(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //}).AddJsonOptions(options =>
            //{
            //    //options.SerializerSettings.DateFormatString = "yyyy年MM月dd日";
            //});
            services.AddMvc();
            services.AddSession();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}).AddCookie(options =>
            //{
            //});
            services.AddAuthentication(options =>
            {
                options.DefaultScheme= CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,

                    ValidIssuer = "zyc",
                    ValidAudience = "all",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("miaomiaoo(=•ェ•=)m"))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Management/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Management}/{action=Index}/{id?}");
            });

            DataInitialization.Initialize(app);
        }
    }
}
