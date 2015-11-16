using System;
using Elm.AspNet.Identity;
using Microsoft.AspNet.Identity;

namespace Microsoft.Framework.DependencyInjection
{
    public static class ElmAspNetIdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddElmIdentity<TUser, TKey>(this IServiceCollection services)
            where TUser : IdentityUser<TUser, TKey>, new()
            where TKey : IEquatable<TKey>
        {
            services.AddScoped<IUserStore<TUser>, UserStore<TUser, TKey>>();
            services.AddScoped<IRoleStore<IdentityRole<TKey>>, RoleStore<IdentityRole<TKey>, TKey>>();
            return services;
        }
    }
}
