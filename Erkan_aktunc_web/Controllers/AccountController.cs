using Erkan_aktunc_web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Erkan_aktunc_web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // --- GİRİŞ YAP (LOGIN) ---
        [HttpGet]
        public IActionResult Login()
        {
            // Eğer kullanıcı zaten giriş yapmışsa direkt projelere at
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Projects");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Lütfen tüm alanları doldurun.";
                return View();
            }

            // Kullanıcıyı bul
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Şifreyi kontrol et
                var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Giriş başarılıysa Projeler sayfasına yönlendir
                    return RedirectToAction("Index", "Projects");
                }
            }

            ViewBag.Error = "Email veya şifre hatalı!";
            return View();
        }

        // --- KAYIT OL (REGISTER) ---
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Projects");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUser model, string password)
        {
            // Kullanıcı nesnesini oluştur
            var user = new ApplicationUser
            {
                UserName = model.Email, // Username email ile aynı olsun
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true // Test için onayı otomatik yapıyoruz
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Varsayılan olarak "Member" rolü ata
                await _userManager.AddToRoleAsync(user, "Member");

                // Kayıt olunca otomatik giriş yapsın ama KALICI OLMASIN (isPersistent: false)
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Projects");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // --- ÇIKIŞ YAP (LOGOUT) ---
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            // Çıkış yapınca Ana Sayfaya (Home) dönsün
            return RedirectToAction("Index", "Home");
        }

        // --- YETKİ YOK SAYFASI ---
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}