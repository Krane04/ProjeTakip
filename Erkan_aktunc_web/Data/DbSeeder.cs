using Erkan_aktunc_web.Models;
using Microsoft.AspNetCore.Identity;

namespace Erkan_aktunc_web.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // 1. Rolleri Oluştur
            await CreateRole(roleManager, "Admin");
            await CreateRole(roleManager, "Manager");
            await CreateRole(roleManager, "Member");

            // 2. Admin Kullanıcısını Oluştur
            var adminEmail = "admin@sakarya.edu.tr"; // Giriş mailin bu olacak
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Erkan",
                    LastName = "Aktunç"
                };

                // Şifre: 123 (Program.cs'de ayarları düşürdüğümüz için kabul eder)
                var result = await userManager.CreateAsync(newAdmin, "123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                    await userManager.AddToRoleAsync(newAdmin, "Manager");
                }
            }
        }

        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}