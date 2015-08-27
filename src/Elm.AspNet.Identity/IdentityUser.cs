using Folke.Orm;
using Folke.Orm.Mapping;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elm.AspNet.Identity
{
    [Table("aspnet_Users")]
    public class IdentityUser : IdentityUser<string>
    {
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        public IdentityUser(string userName) : this()
        {
            UserName = userName;
        }
    }

    [Table("aspnet_Users")]
    public class IdentityUser<TKey> where TKey : IEquatable<TKey>
    {
        public IdentityUser(){}

        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        [Key]
        public TKey Id { get; set; }
        public string UserName { get; set; }
        public virtual string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public virtual string NormalizedEmail { get; set; }
        /// <summary>
        ///     True if the email is confirmed, default is false
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        ///     The salted/hashed form of the user password
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials change (password changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// A random value that should change whenever a user is persisted to the store
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        ///     DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        ///     Is lockout enabled for this user
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        ///     Used to record failures for the purposes of lockout
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        ///     Navigation property for users in the role
        /// </summary>
        [FolkeList(Join = "IdentityRole")]
        public virtual ICollection<IdentityUserRole<TKey>> Roles { get; } = new List<IdentityUserRole<TKey>>();

        /// <summary>
        ///     Navigation property for users claims
        /// </summary>
        [FolkeList(Join = "Claims")]
        public virtual ICollection<IdentityUserClaim<TKey>> Claims { get; } = new List<IdentityUserClaim<TKey>>();

        /// <summary>
        ///     Navigation property for users logins
        /// </summary>
        [FolkeList(Join = "Logins")]
        public virtual ICollection<IdentityUserLogin<TKey>> Logins { get; } = new List<IdentityUserLogin<TKey>>();

    }
}
