using BookSpot.Models;
using Microsoft.AspNetCore.Identity;

namespace BookSpot.Data
{
    public class AppUser : IdentityUser
    {
        public RoleModel AppUserRole { get; set; } = RoleModel.Reader;
    }
}
