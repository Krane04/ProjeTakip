using System.ComponentModel.DataAnnotations;

namespace Erkan_aktunc_web.Models
{
    public enum TaskStatus
    {
        Beklemede,
        Yapiliyor,
        Tamamlandi
    }

    public class ProjectTask
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Beklemede;

        // Hangi Projeye Ait?
        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }

        // Kime Atandı?
        public string? AssignedToId { get; set; }
        public virtual ApplicationUser? AssignedTo { get; set; }
    }
}