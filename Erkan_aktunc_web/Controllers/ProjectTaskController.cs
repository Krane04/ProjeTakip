using Erkan_aktunc_web.Models;
using Erkan_aktunc_web.Repositories;
using Erkan_aktunc_web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Erkan_aktunc_web.Controllers
{
    [Authorize]
    public class ProjectTaskController : Controller
    {
        private readonly IProjectTaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectTaskController(IProjectTaskRepository taskRepository, IProjectRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
        }

        // 1. GÖREV EKLEME SAYFASI (GET)
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(int projectId)
        {
            var project = await _projectRepository.GetByIdWithDetailsAsync(projectId);
            if (project == null) return NotFound();

            ViewBag.Members = new SelectList(project.Members.Select(u => new
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName + " (" + u.Email + ")"
            }), "Id", "FullName");

            return View(new ProjectTaskViewModel { ProjectId = projectId });
        }

        // GÖREV EKLEME İŞLEMİ (POST)
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(ProjectTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var task = new ProjectTask
                {
                    Title = model.Title,
                    Description = model.Description,
                    Status = Models.TaskStatus.Beklemede, // Varsayılan durum
                    ProjectId = model.ProjectId,
                    AssignedToId = model.AssignedToId
                };

                await _taskRepository.AddAsync(task);
                return RedirectToAction("Details", "Projects", new { id = model.ProjectId });
            }

            // Hata varsa sayfayı tekrar doldur
            var project = await _projectRepository.GetByIdWithDetailsAsync(model.ProjectId);
            ViewBag.Members = new SelectList(project.Members.Select(u => new { Id = u.Id, FullName = u.FirstName + " " + u.LastName }), "Id", "FullName");
            return View(model);
        }

        // GÖREV DÜZENLEME (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return NotFound();

            // YETKİ KONTROLÜ: Sadece Admin/Manager veya Görevin Sahibi düzenleyebilir

            var project = await _projectRepository.GetByIdWithDetailsAsync(task.ProjectId);

            ViewBag.Members = new SelectList(project.Members.Select(u => new
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName
            }), "Id", "FullName", task.AssignedToId);

            var model = new ProjectTaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                ProjectId = task.ProjectId,
                AssignedToId = task.AssignedToId
            };

            return View(model);
        }

        //  GÖREV GÜNCELLEME (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(ProjectTaskViewModel model)
        {
            var task = await _taskRepository.GetByIdAsync(model.Id);
            if (task == null) return NotFound();

            // Güncelleme
            task.Title = model.Title;
            task.Description = model.Description;
            task.Status = model.Status;
            task.AssignedToId = model.AssignedToId;

            await _taskRepository.UpdateAsync(task);
            return RedirectToAction("Details", "Projects", new { id = task.ProjectId });
        }

        // GÖREV SİLME
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task != null)
            {
                await _taskRepository.DeleteAsync(id);
                return RedirectToAction("Details", "Projects", new { id = task.ProjectId });
            }
            return NotFound();
        }
    }
}