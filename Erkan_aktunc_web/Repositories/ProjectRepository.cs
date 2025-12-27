using Erkan_aktunc_web.Data;
using Erkan_aktunc_web.Models;
using Microsoft.EntityFrameworkCore;

namespace Erkan_aktunc_web.Repositories
{
   
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Manager)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.Members)
                .Where(p => p.ManagerId == userId || p.Members.Any(m => m.Id == userId))
                .ToListAsync();
        }

        public async Task<Project?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.Members)
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.AssignedTo)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}