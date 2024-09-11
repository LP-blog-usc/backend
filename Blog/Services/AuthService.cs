namespace Blog.Services
{
    using AutoMapper;
    using Blog.Data;
    using Blog.Models.DataSet;
    using Blog.Models.Dtos.Response;
    using Blog.Services.IServices;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Cryptography;
    using System.Text;

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public AuthService(ApplicationDbContext context, IMapper mapper, IRoleService roleService)
        {
            _context = context;
            _mapper = mapper;
            _roleService = roleService;
        }

        public async Task<LoginResponseDto> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !VerifyPassword(password, user.Password))
            {
                return null;
            }

            if (user.Role == null)
            {
                return null;
            }

            return new LoginResponseDto
            {
                RoleId = user.Role.Id,
                UserId = user.Id
            };
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return inputPassword == storedPassword;
        }
    }
}
