using Microsoft.EntityFrameworkCore;
using TheNextEventAPI.Data;
using TheNextEventAPI.DTOs;
using TheNextEventAPI.Models;
using BCrypt.Net;

namespace TheNextEventAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> ValidateTokenAsync(string token);
        Task<AdminUserDto> GetUserByIdAsync(int userId);
        Task<AdminUserDto> UpdateUserAsync(int userId, UpdateUserDto updateDto);
        Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task DeleteUserAsync(int userId);
        Task<List<AdminUserDto>> GetAllUsersAsync();
        
        // Legacy methods
        Task<AdminUserDto> RegisterAsync(RegisterAdminDto registerDto);
        Task<AdminUserDto> LoginAsync(string email, string password);
        Task<AdminUserDto> GetCurrentUserAsync(int userId);
        Task<AdminUserDto> UpdateUserAsync(int userId, UpdateAdminDto updateDto);
        Task<bool> ChangePasswordLegacyAsync(int userId, ChangePasswordDto changePasswordDto);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(ApplicationDbContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AdminUserDto> RegisterAsync(RegisterAdminDto registerDto)
        {
            // Check if user already exists
            var existingUser = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

            if (existingUser != null)
            {
                throw new ArgumentException("User with this email already exists");
            }

            // Create new admin user
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            
            var adminUser = new AdminUser
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PasswordHash = hashedPassword,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.AdminUsers.Add(adminUser);
            await _context.SaveChangesAsync();

            return new AdminUserDto
            {
                Id = adminUser.Id,
                Email = adminUser.Email,
                FirstName = adminUser.FirstName,
                LastName = adminUser.LastName,
                IsActive = adminUser.IsActive,
                CreatedAt = adminUser.CreatedAt,
                LastLoginAt = adminUser.LastLoginAt
            };
        }

        public async Task<AdminUserDto> LoginAsync(string email, string password)
        {
            var user = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new AdminUserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }

        public async Task<AdminUserDto> GetCurrentUserAsync(int userId)
        {
            var user = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            return new AdminUserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }

        public async Task<List<AdminUserDto>> GetAllUsersAsync()
        {
            var users = await _context.AdminUsers
                .Where(u => u.IsActive)
                .OrderBy(u => u.Email)
                .Select(u => new AdminUserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt
                })
                .ToListAsync();

            return users;
        }

        public async Task<AdminUserDto> UpdateUserAsync(int userId, UpdateAdminDto updateDto)
        {
            var user = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Check if email is being changed and if it already exists
            if (updateDto.Email != user.Email)
            {
                var existingUser = await _context.AdminUsers
                    .FirstOrDefaultAsync(u => u.Email == updateDto.Email && u.Id != userId);

                if (existingUser != null)
                {
                    throw new ArgumentException("Email already exists");
                }
            }

            user.Email = updateDto.Email;
            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;

            await _context.SaveChangesAsync();

            return new AdminUserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Soft delete by deactivating
            user.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Current password is incorrect");
            }

            // Update password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ChangePasswordLegacyAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            await ChangePasswordAsync(userId, changePasswordDto);
            return true;
        }

        // Additional methods for new interface compatibility
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await LoginAsync(loginDto.Email, loginDto.Password);
            var token = _jwtTokenService.GenerateToken(user.Id, user.Email, new List<string> { "Admin" });

            return new AuthResponseDto
            {
                Token = token,
                User = user,
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var registerAdminDto = new RegisterAdminDto
            {
                Email = registerDto.Email,
                Password = registerDto.Password,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            var user = await RegisterAsync(registerAdminDto);
            var token = _jwtTokenService.GenerateToken(user.Id, user.Email, new List<string> { "Admin" });

            return new AuthResponseDto
            {
                Token = token,
                User = user,
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var claims = _jwtTokenService.ValidateToken(token);
                return claims != null && !_jwtTokenService.IsTokenExpired(token);
            }
            catch
            {
                return false;
            }
        }

        public async Task<AdminUserDto> GetUserByIdAsync(int userId)
        {
            return await GetCurrentUserAsync(userId);
        }

        public async Task<AdminUserDto> UpdateUserAsync(int userId, UpdateUserDto updateDto)
        {
            var updateAdminDto = new UpdateAdminDto
            {
                Email = updateDto.Email,
                FirstName = updateDto.FirstName,
                LastName = updateDto.LastName
            };

            return await UpdateUserAsync(userId, updateAdminDto);
        }
    }
}
