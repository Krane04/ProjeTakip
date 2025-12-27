using System.ComponentModel.DataAnnotations;

namespace Erkan_aktunc_web.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Proje başlığı zorunludur.")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Proje Yöneticisi (Manager)
        public string? ManagerId { get; set; }
        public ApplicationUser? Manager { get; set; }

        // Projedeki Üyeler (Çoka-Çok İlişki)
        public virtual ICollection<ApplicationUser> Members { get; set; } = new List<ApplicationUser>();

        // Projenin Görevleri
        public virtual ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}