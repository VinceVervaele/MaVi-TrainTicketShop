using Microsoft.AspNetCore.Identity;

namespace Trains_FSD.Areas.Data
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
