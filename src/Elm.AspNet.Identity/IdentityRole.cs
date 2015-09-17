using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Folke.Elm;

namespace Elm.AspNet.Identity
{
    [Table("aspnet_Roles")]
    public class IdentityRole : IdentityRole<string>
    {
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    [Table("aspnet_Roles")]
    public class IdentityRole<TKey> where TKey : IEquatable<TKey>
    {
        public IdentityRole()
            : this("")
        {
        }

        public IdentityRole(string roleName)
        {
            Name = roleName;
        }

        [Key]
        public TKey Id { get; set; }
        public string Name { get; set; }
    }
}
