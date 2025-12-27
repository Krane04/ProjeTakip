using Microsoft.AspNetCore.Identity;

namespace Erkan_aktunc_web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Bir kullanıcının dahil olduğu projeler
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}