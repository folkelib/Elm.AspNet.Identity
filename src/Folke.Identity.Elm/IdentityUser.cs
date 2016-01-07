using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Folke.Elm.Mapping;

namespace Folke.Identity.Elm
{
    /// <summary>
    /// An implementation that can be used as is.
    /// Note that it is sealed. If you need to add custom members
    /// to the class, you can't use this class.
    /// </summary>
    [Table("aspnet_Users")]
    public sealed class IdentityUser : IdentityUser<IdentityUser, string>
    {
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }
    }

    [Table("aspnet_Users")]
    public sealed class IdentityUser<TKey> : IdentityUser<IdentityUser<TKey>, TKey>
        where TKey : IEquatable<TKey>
    {
        
    }

    [Table("aspnet_Users")]
    public class IdentityUser<TUser, TKey> 
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TUser, TKey>
    {
        public IdentityUser()
        {
            Claims = new List<IdentityUserClaim<TUser, TKey>>();
            Roles = new List<IdentityUserRole<TUser, TKey>>();
            Logins = new List<IdentityUserLogin<TUser, TKey>>();
        }

        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        [Key]
        public TKey Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool TwoFactorEnabled { get; set; }
        [Select(IncludeReference = "IdentityRole")]
        public IList<IdentityUserRole<TUser, TKey>> Roles { get; set; }
        [Select(IncludeReference = "Claims")]
        public IList<IdentityUserClaim<TUser, TKey>> Claims { get; set; }
        [Select(IncludeReference = "Logins")]
        public IList<IdentityUserLogin<TUser, TKey>> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string NormalizedUserName { get; set; }
        public string NormalizedEmail { get; set; }
    }
}
