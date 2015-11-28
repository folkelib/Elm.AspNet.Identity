using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Folke.Identity.Elm
{
    [Table("aspnet_IdentityUserRole")]
    public class IdentityUserRole<TUser, TKey>
        where TKey: IEquatable<TKey>
        where TUser: IdentityUser<TUser, TKey>
    {
        [Key]
        public int Id { get; set; }
        public IdentityRole<TKey> Role { get; set; }
        public TUser User { get; set; }
    }
}