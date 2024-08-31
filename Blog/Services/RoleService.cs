using Blog.Data;
using Blog.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Blog.Services.IServices;
using Blog.Models.DataSet;

namespace Blog.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return null;
            }

            return role;
        }

        public async Task<string> GetRoleName(int id)
        {
            var role = await GetRoleByIdAsync(id);

            if (role == null)
            {
                return null;
            }

            return role.Name;
        }
    }
}
