using Folke.Elm;
using Folke.Elm.Sqlite;
using Folke.Identity.Elm.Sample.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;

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

            services.AddElmIdentity<ApplicationUser>();
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    // .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles()
                .UseIdentity()
                .UseFacebookAuthentication(new FacebookOptions {
                    AppId = "901611409868059",
                    AppSecret = "4aa3c530297b1dcebc8860334b39668b"
                })
                .UseGoogleAuthentication(new GoogleOptions {
                    ClientId = "514485782433-fr3ml6sq0imvhi8a7qir0nb46oumtgn9.apps.googleusercontent.com",
                    ClientSecret = "V2nDD9SkFbvLTqAUBWBBxYAL"
                })
                .UseTwitterAuthentication(new TwitterOptions { 
                    ConsumerKey = "BSdJJ0CrDuvEhpkchnukXZBUv",
                    ConsumerSecret = "xKUNuKhsRdHD03eLn67xhPAyE1wFFEndFo1X2UJaK2m1jdAxf4"
                })
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
