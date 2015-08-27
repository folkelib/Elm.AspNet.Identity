using System;
using Folke.Orm;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Elm.AspNet.Identity
{
    /// <summary>
    ///     Represents a Role entity
    /// </summary>
    [Table("aspnet_Roles")]
    public class IdentityRole : IdentityRole<string>
    {
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    /// <summary>
    ///     Represents a Role entity
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    [Table("aspnet_Roles")]
    public class IdentityRole<TKey> where TKey : IEquatable<TKey>
    {
        public IdentityRole() {}

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="roleName"></param>
        public IdentityRole(string roleName) : this()
        {
            Name = roleName;
        }

        /// <summary>
        ///     Navigation property for users in the role
        /// </summary>
        public virtual ICollection<IdentityUserRole<TKey>> Users { get; } = new List<IdentityUserRole<TKey>>();

        /// <summary>
        ///     Navigation property for claims in the role
        /// </summary>
        public virtual ICollection<IdentityRoleClaim<TKey>> Claims { get; } = new List<IdentityRoleClaim<TKey>>();

        [Key]
        public TKey Id { get; set; }
        public string Name { get; set; }
    }
}
