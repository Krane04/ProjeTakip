using Erkan_aktunc_web.Models;
using Erkan_aktunc_web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Erkan_aktunc_web.Controllers
{
    [Authorize] 
    public class ProjectsController : Controller
    {
        private readonly IProjectRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectsController(IProjectRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        // 1. LİSTELEME (KISITLAMALI)
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            IEnumerable<Project> projects;

            // Eğer Admin ise HER ŞEYİ görsün
            if (User.IsInRole("Admin"))
            {
                projects = await _repository.GetAllAsync();
            }
            else
            {
                // Manager veya Member ise sadece kendi ilgilendiklerini görsün
                projects = await _repository.GetAllByUserIdAsync(userId);
            }

            return View(projects);
        }

        // 2. DETAY (Dropdown Listesi Hazırlığı İle)
        public async Task<IActionResult> Details(int id)
        {
            var project = await _repository.GetByIdWithDetailsAsync(id);
            if (project == null) return NotFound();

            // --- DROPDOWN İÇİN LİSTE HAZIRLAMA ---
            var allUsers = await _userManager.Users.ToListAsync();

            // Zaten projede ekli olanları listeden çıkart
            var existingMemberIds = project.Members.Select(m => m.Id).ToList();

            // Proje Yöneticisini de listeden çıkart
            existingMemberIds.Add(project.ManagerId);

            var eligibleUsers = allUsers
                .Where(u => !existingMemberIds.Contains(u.Id)) // Zaten ekli olmayanlar
                .Select(u => new
                {
                    Id = u.Id,
                    DisplayText = $"{u.FirstName} {u.LastName} ({u.Email})" // İsim + Email
                })
                .ToList();

            // Listeyi View'a taşıyoruz
            ViewBag.PotentialMembers = new SelectList(eligibleUsers, "Id", "DisplayText");

            return View(project);
        }

        // 3. YENİ PROJE OLUŞTURMA (Sadece Yönetici ve Admin)
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(Erkan_aktunc_web.ViewModels.ProjectViewModel model)
        {
            // Validasyon kontrolü
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = new Project
            {
                Title = model.Title,
                Description = model.Description,
                CreatedDate = DateTime.Now,
                ManagerId = _userManager.GetUserId(User) // Oluşturan kişi yönetici olur
            };

            await _repository.AddAsync(project);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AddMember(int projectId, string userId)
        {
            var project = await _repository.GetByIdWithDetailsAsync(projectId);
            if (project == null) return NotFound();

            // GÜVENLİK: Sadece Admin veya o projenin Yöneticisi üye ekleyebilir
            var currentUserId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && project.ManagerId != currentUserId)
            {
                return Forbid(); // Yetkisiz erişim
            }

            // Dropdown'dan boş gelirse
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Lütfen listeden bir kullanıcı seçin.";
                return RedirectToAction(nameof(Details), new { id = projectId });
            }

            var userToAdd = await _userManager.FindByIdAsync(userId);

            if (userToAdd != null)
            {
                // Zaten ekli mi kontrolü
                if (!project.Members.Any(u => u.Id == userToAdd.Id))
                {
                    project.Members.Add(userToAdd);
                    await _repository.UpdateAsync(project);
                    TempData["Success"] = $"{userToAdd.FirstName} {userToAdd.LastName} projeye eklendi.";
                }
                else
                {
                    TempData["Error"] = "Bu kullanıcı zaten projede ekli.";
                }
            }
            else
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
            }

            return RedirectToAction(nameof(Details), new { id = projectId });
        }

        // 5. PROJE SİLME (Sadece Yönetici ve Admin)
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}