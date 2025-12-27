using Erkan_aktunc_web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Erkan_aktunc_web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Çoka-Çok İlişki (PROJE <-> ÜYELER)
            builder.Entity<Project>()
                .HasMany(p => p.Members)
                .WithMany(u => u.Projects)
                .UsingEntity(j => j.ToTable("ProjectMembers"));

            // 2. Bire-Çok İlişki (PROJE <-> YÖNETİCİ) --- HATAYI ÇÖZEN KISIM ---
            builder.Entity<Project>()
                .HasOne(p => p.Manager)
                .WithMany() // Kullanıcı tarafında "YönettiğimProjeler" listesi olmadığı için boş bırakıyoruz
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // Yönetici silinirse proje silinmesin diye koruma
        }
    }
}