using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Exceptions;
using ServiceStationAPI.Models;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceStationAPI.Services
{
    public interface IAccountService
    {
        Task RegisterAccount(RegisterAccountDto dto);
        Task<string> GenerateJwtToken(LoginAccountDto dto);
        Task UpdateAccount(string email, UpdateAccountDto dto);
        Task<IEnumerable<AccountDto>> GetAccounts();
        Task<AccountDto> GetAccount(string email);
        Task DeleteAccount(string email);
    }
    public class AccountService:IAccountService
    {
        private readonly IMapper _mapper;
        private readonly ServiceStationDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly ILogger<AccountService> _logger;
        private readonly IUserContextService _userContextService;
        
        public AccountService(ServiceStationDbContext dbContext,IMapper mapper,IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings,
            ILogger<AccountService> logger, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _logger = logger;
            _userContextService = userContextService;
           

        }
        public async Task RegisterAccount(RegisterAccountDto dto)
        {
            var newAccount = _mapper.Map<User>(dto);
            var hashedPassword = _passwordHasher.HashPassword(newAccount,dto.Password);
            newAccount.PasswordHash = hashedPassword;
            await _dbContext.Users.AddAsync(newAccount);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"New uesr with Id {newAccount.Id} registered");
        }

        public async Task<string> GenerateJwtToken(LoginAccountDto dto)
        {
            var user = await _dbContext.Users.Include(u=>u.Role).FirstOrDefaultAsync(u=>u.Email == dto.Email);
            if (user == null)
                throw new BadRequestException("Incorrect email or password");
            var isPasswordValid = _passwordHasher.VerifyHashedPassword(user,user.PasswordHash ,dto.Password);
            if(isPasswordValid == PasswordVerificationResult.Failed)
                throw new BadRequestException("Incorrect email or password");
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,$"{user.Name} {user.Surname}"),
                new Claim(ClaimTypes.Role,user.Role.Name),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            
            var expires = DateTime.Now.AddMinutes(_authenticationSettings.JwtExpireMins);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,_authenticationSettings.JwtIssuer,claims,
                expires:expires, signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            _logger.LogInformation($"User with Id {user.Id} logged in");
            return tokenHandler.WriteToken(token);
        }

        public async Task UpdateAccount(string email,UpdateAccountDto dto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new NotFoundException("User not found");
            if (user.RoleId == 3)
                throw new ForbiddenException("Permission denied");
            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.PhoneNumber = dto.PhoneNumber;
            user.RoleId = dto.RoleId;
            _logger.LogInformation($"Account with email {email} updated");
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AccountDto>> GetAccounts()
        {
            var accounts = await _dbContext.Users.Include(u=>u.Role).Include(u=>u.Vehicles).Include(u=>u.OrderNotes).ToListAsync();
            var dtos = _mapper.Map<List<AccountDto>>(accounts);
            return dtos;
        }

        public async Task<AccountDto> GetAccount(string email)
        {
            var account = await _dbContext.Users.Include(u => u.Role).Include(u => u.Vehicles).Include(u => u.OrderNotes).FirstOrDefaultAsync(u=>u.Email == email);
            if (account is null)
                throw new NotFoundException("Account not found");
            var dto = _mapper.Map<AccountDto>(account);
            return dto;
        }

        public async Task DeleteAccount(string email)
        {
            var account = await _dbContext.Users.FirstOrDefaultAsync(u=>u.Email == email);
            if (account is null)
                throw new NotFoundException("Account not found");
            if (account.RoleId == 3)
            {
                _logger.LogInformation($"User with Id {_userContextService.GetUserId} tried to remove user with Id {account.Id}");
                throw new ForbiddenException("Permission denied");
            }
            _dbContext.Remove(account);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Account with email {email} deleted");
        }
    }
}
