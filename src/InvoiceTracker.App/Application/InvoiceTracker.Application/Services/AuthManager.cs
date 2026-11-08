using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Application.Interfaces.Services;
using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Models.InvoiceTracker.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InvoiceTracker.Application.Services
{
    public class AuthManager : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;
        private readonly IEncryptionService _encryptionService;

        public AuthManager(
                           IConfiguration configuration,
                           ILogger<AuthManager> logger,
                           IEncryptionService encryptionService,
                           IUserRepository userRepository,
                           IRoleRepository roleRepository,
                           ISettingRepository settingRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _settingRepository = settingRepository;
            _configuration = configuration;
            _logger = logger;
            _encryptionService = encryptionService;
        }
        public Task<List<UserRole>> GetUserRoles(int userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponse> LoginRequest(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(loginRequest.Email, cancellationToken);
            if (user is null)
            {
                _logger.LogWarning($"Login failed for {loginRequest.Email}: user not found or inactive.");
                return new LoginResponse(false, "", null, "User not found or inactive", DateTime.MinValue);
            }

            var password = await _encryptionService.Decrypt(user.PasswordHash);

            if (loginRequest.Password != password)
            {
                _logger.LogWarning($"Login failed for : {loginRequest.Email}: invalid password");
                return new LoginResponse(false, "", null, "Invalid password", DateTime.MinValue);

            }

            var role = await _roleRepository.GetUserRole(user.RoleId);

            if (role is null)
            {
                _logger.LogWarning($"No role assign to for : {loginRequest.Email}");
                return new LoginResponse(false, "", null, "User does not have role Assigned", DateTime.MinValue);
            }

            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key is not configured.");
            var issuer = _configuration["Jwt:Issuer"];

            var expirationMinutes = int.TryParse(
                _configuration["Jwt:ExpirationInMinutes"],
                out var configuredExpirationMinutes)
                ? configuredExpirationMinutes
                : 60;
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email.ToString()),
                new(JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}".Trim()),
                new(ClaimTypes.Role, role.RoleName)

            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Convert.FromBase64String(key)),
                SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            user.LastLoginUtc = DateTime.UtcNow;

            _ = await _userRepository.UpdateUser(user);

            return new LoginResponse(
                true,
                token,
                role,
                "Login successful",
                expiresAt);

        }

        public async Task<RegisterUserResponse> Register(
            RegisterUserRequest registerUser,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(registerUser);
            ArgumentException.ThrowIfNullOrWhiteSpace(registerUser.FirstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(registerUser.LastName);
            ArgumentException.ThrowIfNullOrWhiteSpace(registerUser.Email);
            ArgumentException.ThrowIfNullOrWhiteSpace(registerUser.Password);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(registerUser.RoleId);

            var email = registerUser.Email.Trim();

            if (await _userRepository.UserExistsAsync(email, cancellationToken))
            {
                return new RegisterUserResponse
                {
                    Success = false,
                    Message = "User with this email already exists."
                };
            }

            var user = new User
            {
                FirstName = registerUser.FirstName.Trim(),
                LastName = registerUser.LastName.Trim(),
                Email = email.Trim(),
                PasswordHash = await _encryptionService.Encrypt(registerUser.Password),
                IsActive = true,
                IsLocked = false,
                RoleId = registerUser.RoleId,
                CreatedBy = email,
                CreatedDate = DateTime.UtcNow
            };

            await _userRepository.AddUser(user, cancellationToken);

            return new RegisterUserResponse
            {
                Success = true,
                Message = "User registered successfully."
            };
        }
    }
}
