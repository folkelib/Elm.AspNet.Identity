using Folke.Elm;
using Folke.Elm.Sqlite;
using Folke.Identity.Elm.Sample.Models;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Folke.Identity.Elm.Sample
{
    public class Startup
    {
        public Startup()
        {
            var applicationEnvironment = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application;
            /*
            * Below code demonstrates usage of multiple configuration sources. For instance a setting say 'setting1' is found in both the registered sources,
            * then the later source will win. By this way a Local config can be overridden by a different setting while deployed remotely.
            */
            var builder = new ConfigurationBuilder()
                .SetBasePath(applicationEnvironment.ApplicationBasePath)
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
                .UseFacebookAuthentication(options =>
                {
                    options.AppId = "901611409868059";
                    options.AppSecret = "4aa3c530297b1dcebc8860334b39668b";
                })
                .UseGoogleAuthentication(options =>
                {
                    options.ClientId = "514485782433-fr3ml6sq0imvhi8a7qir0nb46oumtgn9.apps.googleusercontent.com";
                    options.ClientSecret = "V2nDD9SkFbvLTqAUBWBBxYAL";
                })
                .UseTwitterAuthentication(options =>
                {
                    options.ConsumerKey = "BSdJJ0CrDuvEhpkchnukXZBUv";
                    options.ConsumerSecret = "xKUNuKhsRdHD03eLn67xhPAyE1wFFEndFo1X2UJaK2m1jdAxf4";
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
