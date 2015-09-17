using System.Diagnostics;
using Elm.AspNet.Identity.Sample.Models;
using Folke.Elm;
using Folke.Elm.Mapping;
using Folke.Elm.Sqlite;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace Elm.AspNet.Identity.Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment applicationEnvironment)
        {
            /*
            * Below code demonstrates usage of multiple configuration sources. For instance a setting say 'setting1' is found in both the registered sources,
            * then the later source will win. By this way a Local config can be overridden by a different setting while deployed remotely.
            */
            var builder = new ConfigurationBuilder(applicationEnvironment.ApplicationBasePath)
                .AddJsonFile("LocalConfig.json")
                .AddEnvironmentVariables(); //All environment variables in the process's context flow in as configuration values.

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["Data:IdentityConnection:ConnectionString"];
            Debug.Assert(connectionString != null);
            services.AddSingleton<IDatabaseDriver, SqliteDriver>();
            services.AddSingleton<IMapper, Mapper>();
            services.AddScoped<IFolkeConnection>(provider => new FolkeConnection(provider.GetService<IDatabaseDriver>(), provider.GetService<IMapper>(), connectionString));
        //services.AddEntityFramework()
        //        .AddSqlServer()
        //        .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:IdentityConnection:ConnectionString"]));
        services.Configure<IdentityDbContextOptions>(options =>
        {
            options.DefaultAdminUserName = Configuration["DefaultAdminUsername"];
            options.DefaultAdminPassword = Configuration["DefaultAdminPassword"];
        });
            services.AddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            services.AddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>();

        var session = new FolkeConnection(new SqliteDriver(), new Mapper(), connectionString);
        session.UpdateStringIdentityUserSchema<ApplicationUser>();
        session.UpdateStringIdentityRoleSchema();

        services.AddIdentity<ApplicationUser, IdentityRole>()
                    // .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureFacebookAuthentication(options =>
            {
                options.AppId = "901611409868059";
                options.AppSecret = "4aa3c530297b1dcebc8860334b39668b";
            });
            services.ConfigureGoogleAuthentication(options =>
            {
                options.ClientId = "514485782433-fr3ml6sq0imvhi8a7qir0nb46oumtgn9.apps.googleusercontent.com";
                options.ClientSecret = "V2nDD9SkFbvLTqAUBWBBxYAL";
            });
            services.ConfigureTwitterAuthentication(options =>
            {
                options.ConsumerKey = "BSdJJ0CrDuvEhpkchnukXZBUv";
                options.ConsumerSecret = "xKUNuKhsRdHD03eLn67xhPAyE1wFFEndFo1X2UJaK2m1jdAxf4";
            });
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles()
                .UseIdentity()
                .UseFacebookAuthentication()
                .UseGoogleAuthentication()
                .UseTwitterAuthentication()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
                });

            //Populates the Admin user and role
            SampleData.InitializeIdentityDatabaseAsync(app.ApplicationServices).Wait();
        }
    }
}
