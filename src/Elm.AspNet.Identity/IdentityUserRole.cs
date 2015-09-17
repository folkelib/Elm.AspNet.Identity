using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elm.AspNet.Identity
{
    [Table("aspnet_IdentityUserRole")]
    public class IdentityUserRole<TKey>
        where TKey: IEquatable<TKey>
    {
        [Key]
        public int Id { get; set; }
        public IdentityRole<TKey> Role { get; set; }
        public IdentityUser<TKey> User { get; set; }
    }
}