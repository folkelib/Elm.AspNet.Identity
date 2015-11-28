using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using Folke.Elm;

namespace Folke.Identity.Elm.Sample.Models
{
    public static class SampleData
    {
        public static async Task InitializeIdentityDatabaseAsync(IServiceProvider serviceProvider)
        {
            using (var transaction = serviceProvider.GetRequiredService<IFolkeConnection>().BeginTransaction())
            {
                await CreateAdminUser(serviceProvider);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Creates a store manager user who can manage the inventory.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<IdentityDbContextOptions>>();

            const string adminRole = "Administrator";

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole { Name =  adminRole });
            }

            var user = await userManager.FindByNameAsync(options.Value.DefaultAdminUserName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = options.Value.DefaultAdminUserName };
                await userManager.CreateAsync(user, options.Value.DefaultAdminPassword);
                await userManager.AddToRoleAsync(user, adminRole);
                await userManager.AddClaimAsync(user, new Claim("ManageStore", "Allowed"));
            }
        }
    }
}