using Erkan_aktunc_web.Models;
using Erkan_aktunc_web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Erkan_aktunc_web.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin girebilir
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // 1. KULLANICILARI LİSTELE
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRolesViewModel.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CurrentRole = roles.FirstOrDefault() ?? "Rol Yok"
                });
            }

            return View(userRolesViewModel);
        }

        // 2. ROL DEĞİŞTİRME İŞLEMİ
        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // Mevcut rolleri al ve sil
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Yeni rolü ekle
            await _userManager.AddToRoleAsync(user, newRole);

            TempData["Success"] = $"{user.Email} kullanıcısının rolü {newRole} olarak güncellendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            // GÜVENLİK KONTROLÜ: Admin kendi kendini silemesin!
            var currentUserId = _userManager.GetUserId(User);
            if (user.Id == currentUserId)
            {
                TempData["Error"] = "Güvenlik gereği kendi hesabınızı silemezsiniz!";
                return RedirectToAction("Index");
            }

            // Kullanıcıyı sil
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Kullanıcı başarıyla silindi.";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                TempData["Error"] = "Kullanıcı silinirken bir hata oluştu.";
            }

            return RedirectToAction("Index");
        }
    }
}