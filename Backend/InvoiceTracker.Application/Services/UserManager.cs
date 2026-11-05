using InvoiceTracker.Domain.Interfaces.Services;
using InvoiceTracker.Domain.Models.Authentication;
using InvoiceTracker.Domain.Models.User;
using InvoiceTracker.Infrastructure.Persistence;
using InvoiceTracker.Infrastructure.Persistence.DatabaseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace InvoiceTracker.Application.Services
{
    public class UserManager : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserManager> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserManager(
            AppDbContext context,
            IConfiguration configuration,
            ILogger<UserManager> logger,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public Task<List<UserRole>> GetUserRoles(int userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponse> LoginRequest(
            LoginRequest loginRequest,
            CancellationToken cancellationToken)
        {
            var identifier = string.IsNullOrWhiteSpace(loginRequest.Email)
                ? loginRequest.Username
                : loginRequest.Email;

            _logger.LogInformation("Login attempt for {LoginIdentifier}", identifier);

            if (string.IsNullOrWhiteSpace(identifier) ||
                string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                _logger.LogWarning("Login failed because the credentials were incomplete for {LoginIdentifier}", identifier);
                return null!;
            }

            var user = await _context.Users
                .Include(user => user.Role)
                .SingleOrDefaultAsync(
                    user => user.Email == identifier &&
                            user.IsActive &&
                            !user.IsLocked,
                    cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("Login failed for {LoginIdentifier}: user not found, inactive, or locked", identifier);
                return null!;
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                loginRequest.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Login failed for {LoginIdentifier}: invalid password", identifier);
                return null!;
            }

            if (passwordResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(
                    user,
                    loginRequest.Password);
            }

            var expirationMinutes =
                _configuration.GetValue<int?>("Jwt:ExpirationInMinutes") ?? 60;

            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];

            if (string.IsNullOrWhiteSpace(key) ||
                string.IsNullOrWhiteSpace(issuer))
            {
                throw new InvalidOperationException(
                    "JWT configuration is missing Jwt:Key or Jwt:Issuer.");
            }

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Email.ToString()),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim())
            };

            if (user.Role is not null)
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role.RoleName));
            }

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            user.LastLoginUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Login succeeded for {LoginIdentifier}", identifier);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var roles = user.Role is null
                ? new List<UserRole>()
                : new List<UserRole>
                {
                    new(
                        user.Role.RoleId,
                        user.Role.IsActive,
                        user.Role.CreatedDate,
                        user.Role.ModifiedBy,
                        user.Role.ModifiedDate)
                    {
                        RoleName = user.Role.RoleName,
                        Email = user.Email,
                        UserName = $"{user.FirstName} {user.LastName}".Trim(),
                        CreatedBy = user.Role.CreatedBy
                    }
                };

            return new LoginResponse(token, token, roles, expiresAt);
        }

        public async Task<bool> Register(
            RegisterUserRequest registerUser,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(registerUser);

            var existingUser = await _context.Users.FirstOrDefaultAsync(
                user => user.Email == registerUser.Email,
                cancellationToken);

            if (existingUser is not null)
            {
                return false;
            }

            var user = new User
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                PasswordHash = _passwordHasher.HashPassword(
                    new User
                    {
                        FirstName = registerUser.FirstName,
                        LastName = registerUser.LastName,
                        Email = registerUser.Email
                    },
                    registerUser.Password)
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
