using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Folke.Identity.Elm
{
    [Table("aspnet_UserClaims")]
    public class IdentityUserClaim<TUser, TKey> 
        where TUser: IdentityUser<TUser, TKey>
        where TKey : IEquatable<TKey>
    {
        [Key]
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public TUser User { get; set; }
    }
}
