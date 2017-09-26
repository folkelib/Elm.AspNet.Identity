using Folke.Elm;
using Folke.Elm.Sqlite;
using Folke.Identity.Elm.Sample.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Folke.Identity.Elm.Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("LocalConfig.json")
                .AddEnvironmentVariables(); //All environment variables in the process's context flow in as configuration values.

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddElm<SqliteDriver>(options => options.ConnectionString = Configuration["Data:IdentityConnection:ConnectionString"]);
            services.Configure<IdentityDbContextOptions>(options =>
            {
                options.DefaultAdminUserName = Configuration["DefaultAdminUsername"];
                options.DefaultAdminPassword = Configuration["DefaultAdminPassword"];
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddFacebook(o =>
                {
                    o.AppId = "901611409868059";
                    o.AppSecret = "4aa3c530297b1dcebc8860334b39668b";
                })
                .AddGoogle(o =>
                {
                    o.ClientId = "514485782433-fr3ml6sq0imvhi8a7qir0nb46oumtgn9.apps.googleusercontent.com";
                    o.ClientSecret = "V2nDD9SkFbvLTqAUBWBBxYAL";
                })
                .AddTwitter(o =>
                {
                    o.ConsumerKey = "BSdJJ0CrDuvEhpkchnukXZBUv";
                    o.ConsumerSecret = "xKUNuKhsRdHD03eLn67xhPAyE1wFFEndFo1X2UJaK2m1jdAxf4";
                });

            services.AddElmIdentity<ApplicationUser>();
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    // .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles()
                .UseAuthentication()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
                });

            var session = app.ApplicationServices.GetService<IFolkeConnection>();
            session.UpdateStringIdentityUserSchema<ApplicationUser>();
            session.UpdateStringIdentityRoleSchema();

            //Populates the Admin user and role
            SampleData.InitializeIdentityDatabaseAsync(app.ApplicationServices).Wait();
        }
    }
}
