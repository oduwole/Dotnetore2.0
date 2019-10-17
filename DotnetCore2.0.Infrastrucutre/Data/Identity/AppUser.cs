using Microsoft.AspNetCore.Identity;

namespace DotnetCore2.Infrastrucutre.Data.Identity
{
    public class AppUser : IdentityUser
    {
        // Add additional profile data for application users by adding properties to this class
        public string Name { get; set; }        
    }

    public class AppRole : IdentityRole
    {
        public AppRole(string roleName) : base(roleName)
        {
        }

        // Add additional profile data for application users by adding properties to this class
        public string Description { get; set; }
    }
}
