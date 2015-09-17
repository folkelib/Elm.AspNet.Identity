using System;

namespace Elm.AspNet.Identity
{
    public class RoleUser
    {

        public RoleUser()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        public IdentityRole IdentityRole { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}