using Microsoft.AspNetCore.Identity;

namespace lanchonete.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
