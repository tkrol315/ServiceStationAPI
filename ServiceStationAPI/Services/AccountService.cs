using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Exceptions;
using ServiceStationAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceStationAPI.Services
{
    public interface IAccountService
    {
        void RegisterAccount(RegisterAccountDto dto);
        string GenerateJwtToken(LoginAccountDto dto);
    }
    public class AccountService:IAccountService
    {
        private readonly IMapper _mapper;
        private readonly ServiceStationDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly ILogger<AccountService> _logger;
        public AccountService(ServiceStationDbContext dbContext,IMapper mapper,IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings,
            ILogger<AccountService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _logger = logger;
        }
        public void RegisterAccount(RegisterAccountDto dto)
        {
            var newAccount = _mapper.Map<User>(dto);
            var hashedPassword = _passwordHasher.HashPassword(newAccount,dto.Password);
            newAccount.PasswordHash = hashedPassword;
            _dbContext.Users.Add(newAccount);
            _dbContext.SaveChanges();
            _logger.LogInformation($"New uesr with Id {newAccount.Id} registered");
        }

        public string GenerateJwtToken(LoginAccountDto dto)
        {
            var user = _dbContext.Users.Include(u=>u.Role).FirstOrDefault(u=>u.Email == dto.Email);
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
            //TEMPORARY CHANGE
            //var expires = DateTime.Now.AddMinutes(_authenticationSettings.JwtExpireMins);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireMins);
            //==============================================================================

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,_authenticationSettings.JwtIssuer,claims,
                expires:expires, signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            _logger.LogInformation($"User with Id {user.Id} logged in");
            return tokenHandler.WriteToken(token);
        }
    }
}
