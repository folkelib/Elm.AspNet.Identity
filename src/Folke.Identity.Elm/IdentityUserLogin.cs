using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Folke.Identity.Elm
{
    [Table("aspnet_UserLogins")]
    public class IdentityUserLogin<TUser, TKey> 
        where TUser : IdentityUser<TUser, TKey>
        where TKey: IEquatable<TKey>
    {
        [Key]
        public int Id { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public TUser User { get; set; }
        public string ProviderDisplayName { get; set; }
    }
}
