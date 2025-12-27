using System.ComponentModel.DataAnnotations;
using Erkan_aktunc_web.Models;

namespace Erkan_aktunc_web.ViewModels
{
    public class ProjectTaskViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Görev Başlığı")]
        [Required(ErrorMessage = "Lütfen bir başlık giriniz.")]
        public string Title { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Durum")]
        public Erkan_aktunc_web.Models.TaskStatus Status { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Display(Name = "Kime Atanacak?")]
        public string? AssignedToId { get; set; }
    }
}