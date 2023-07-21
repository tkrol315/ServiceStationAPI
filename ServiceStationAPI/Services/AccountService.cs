using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Models;

namespace ServiceStationAPI.Services
{
    public interface IAccountService
    {
        void RegisterAccount(RegisterAccountDto dto);
    }
    public class AccountService:IAccountService
    {
        private readonly IMapper _mapper;
        private readonly ServiceStationDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        public AccountService(ServiceStationDbContext dbContext,IMapper mapper,IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;

        }
        public void RegisterAccount(RegisterAccountDto dto)
        {
            var newAccount = _mapper.Map<User>(dto);
            var hashedPassword = _passwordHasher.HashPassword(newAccount,dto.Password);
            newAccount.PasswordHash = hashedPassword;
            _dbContext.Users.Add(newAccount);
            _dbContext.SaveChanges();
        }
    }
}
