using Blog.Data;
using Blog.Models;
using Blog.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Models.Dtos.UpdateDtos;
using AutoMapper;
using Blog.Models.DataSet;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;

    public UserService(ApplicationDbContext context, IRoleService roleService, IMapper mapper)
    {
        _context = context;
        _roleService = roleService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserResponseDto>> GetUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        var roles = await _roleService.GetRolesAsync();
        var roleDictionary = roles.ToDictionary(r => r.Id, r => r.Name);

        return _mapper.Map<IEnumerable<UserResponseDto>>(users, opts => opts.Items["RoleDictionary"] = roleDictionary);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return null;
        }

        var roleName = await _roleService.GetRoleName(user.RoleId);
        var roleDictionary = new Dictionary<int, string> { { user.RoleId, roleName } };

        return _mapper.Map<UserResponseDto>(user, opts => opts.Items["RoleDictionary"] = roleDictionary);
    }

    public async Task<bool> UpdateUserAsync(int id, UserUpdateDto userDto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return false;
        }

        // Mapear los datos del DTO a la entidad User
        user = validateUpdateFields(user, userDto);

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return false;
            }
            else
            {
                throw;
            }
        }
    }

    public async Task<UserResponseDto> CreateUserAsync(UserRequestDto userDto)
    {
        // Verificar si el usuario con el mismo email ya existe
        if (await UserExistsByEmailAsync(userDto.Email))
        {
            throw new InvalidOperationException("El correo electrónico ya existe.");
        }

        // Mapear el DTO a la entidad User
        var user = _mapper.Map<User>(userDto);

        // Agregar el usuario a la base de datos
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Mapear la entidad User al DTO de respuesta
        var createdUserDto = _mapper.Map<UserResponseDto>(user);

        return createdUserDto;
    }


    public async Task<UserResponseDto> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return null;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return _mapper.Map<UserResponseDto>(user);
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }

    public async Task<bool> UserExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    private User validateUpdateFields(User user, UserUpdateDto userDto)
    {
        User resultUser = new User();

        if (!string.IsNullOrEmpty(userDto.Name))
        {
            user.Name = userDto.Name;
        }

        if (!string.IsNullOrEmpty(userDto.LastName))
        {
            resultUser.LastName = userDto.LastName;
        }

        if (!string.IsNullOrEmpty(userDto.TelephoneNumber))
        {
            resultUser.TelephoneNumber = userDto.TelephoneNumber;
        }

        if (!string.IsNullOrEmpty(userDto.Email))
        {
            resultUser.Email = userDto.Email;
        }

        if (!string.IsNullOrEmpty(userDto.Password))
        {
            resultUser.Password = userDto.Password;
        }

        if (userDto.RoleId != user.RoleId && userDto.RoleId != 0)
        {
            resultUser.RoleId = userDto.RoleId;
        }

        return resultUser;
    }
}
