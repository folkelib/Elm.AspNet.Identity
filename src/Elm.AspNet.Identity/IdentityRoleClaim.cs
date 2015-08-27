using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Elm.AspNet.Identity
{
    /// <summary>
    ///     EntityType that represents one specific role claim
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class IdentityRoleClaim<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        ///     Primary key
        /// </summary>
        [Key]
        public virtual int Id { get; set; }

        /// <summary>
        ///     User Id for the role this claim belongs to
        /// </summary>
        public virtual TKey RoleId { get; set; }

        /// <summary>
        ///     Claim type
        /// </summary>
        public virtual string ClaimType { get; set; }

        /// <summary>
        ///     Claim value
        /// </summary>
        public virtual string ClaimValue { get; set; }
    }
}
