using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Folke.Elm;

namespace Elm.AspNet.Identity
{
    /// <summary>
    ///     EntityType that represents one specific role claim
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    [Table("aspnet_IdentityRoleClaim")]
    public class IdentityRoleClaim<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        ///     Primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     User Id for the role this claim belongs to
        /// </summary>
        public TKey RoleId { get; set; }

        /// <summary>
        ///     Claim type
        /// </summary>
        public string ClaimType { get; set; }

        /// <summary>
        ///     Claim value
        /// </summary>
        public string ClaimValue { get; set; }
    }
}
