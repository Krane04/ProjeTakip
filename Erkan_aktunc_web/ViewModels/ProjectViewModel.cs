using System.ComponentModel.DataAnnotations;

namespace Erkan_aktunc_web.ViewModels
{
    public class ProjectViewModel
    {
       

        [Display(Name = "Proje Başlığı")]
        [Required(ErrorMessage = "Lütfen bir proje başlığı giriniz.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; }

        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        public string Description { get; set; }
    }
}