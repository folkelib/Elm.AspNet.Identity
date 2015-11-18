namespace Elm.AspNet.Identity.Sample.Models
{
    public class ApplicationUser : IdentityUser<ApplicationUser, string> { }

    public class IdentityDbContextOptions
    {
        public string DefaultAdminUserName { get; set; }

        public string DefaultAdminPassword { get; set; }
    }
}
