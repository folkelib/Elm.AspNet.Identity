using Folke.Orm;

namespace Elm.AspNet.Identity
{
    public static class FolkeConnectionExtensions
    {
        public static void UpdateStringIdentityUserSchema(this IFolkeConnection folkeConnection)
        {
            folkeConnection.CreateOrUpdateTable<IdentityUser>();
            folkeConnection.CreateOrUpdateTable<IdentityUserClaim<IdentityUser, string>>();
            folkeConnection.CreateOrUpdateTable<IdentityUserLogin<IdentityUser, string>>();
        }

        public static void UpdateStringIdentityUserSchema<TUser>(this IFolkeConnection folkeConnection) where TUser : IdentityUser, new()
        {
            folkeConnection.CreateOrUpdateTable<TUser>();
            folkeConnection.CreateOrUpdateTable<IdentityUserClaim<TUser, string>>();
            folkeConnection.CreateOrUpdateTable<IdentityUserLogin<TUser, string>>();
        }

        public static void UpdateStringIdentityRoleSchema(this IFolkeConnection folkeConnection)
        {
            folkeConnection.CreateOrUpdateTable<IdentityRole>();
        }

        public static void UpdateIdentityUserSchema<TKey>(this IFolkeConnection folkeConnection)
        {
            folkeConnection.CreateOrUpdateTable<IdentityUser<TKey>>();
            folkeConnection.CreateOrUpdateTable<IdentityUserClaim<IdentityUser<TKey>, TKey>>();
            folkeConnection.CreateOrUpdateTable<IdentityUserLogin<IdentityUser<TKey>, TKey>>();
        }

        public static void UpdateIdentityUserSchema<TKey, TUser>(this IFolkeConnection folkeConnection) where TUser: IdentityUser<TKey>, new()
        {
            folkeConnection.CreateOrUpdateTable<TUser>();
            folkeConnection.CreateOrUpdateTable<IdentityUserClaim<TUser, TKey>>();
            folkeConnection.CreateOrUpdateTable<IdentityUserLogin<TUser, TKey>>();
        }

        public static void UpdateIdentityRoleSchema<TKey>(this IFolkeConnection folkeConnection)
        {
            folkeConnection.CreateOrUpdateTable<IdentityRole<TKey>>();
        }
    }
}
