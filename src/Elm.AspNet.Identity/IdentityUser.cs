using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Folke.Elm.Mapping;

namespace Elm.AspNet.Identity
{
    [Table("aspnet_Users")]
    public class IdentityUser : IdentityUser<string>
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
    public class IdentityUser<TKey> where TKey : IEquatable<TKey>
    {
        public IdentityUser()
        {
            Claims = new List<IdentityUserClaim<IdentityUser<TKey>, TKey>>();
            Roles = new List<RoleUser>();
            Logins = new List<IdentityUserLogin<IdentityUser<TKey>, TKey>>();
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
        [FolkeList(Join = "IdentityRole")]
        public IList<RoleUser> Roles { get; set; }
        [FolkeList(Join = "Claims")]
        public IList<IdentityUserClaim<IdentityUser<TKey>, TKey>> Claims { get; set; }
        [FolkeList(Join = "Logins")]
        public IList<IdentityUserLogin<IdentityUser<TKey>, TKey>> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string NormalizedUserName { get; set; }
    }
}
