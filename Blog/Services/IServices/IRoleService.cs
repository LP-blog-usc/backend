using Blog.Models.DataSet;

namespace Blog.Services.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task<string> GetRoleName(int id);
    }
}
