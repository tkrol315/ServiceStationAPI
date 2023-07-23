
using Microsoft.AspNetCore.Mvc;
using ServiceStationAPI.Models;
using ServiceStationAPI.Services;

namespace ServiceStationAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController:ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterAccountDto dto)
        {
            _accountService.RegisterAccount(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginAccountDto dto)
        {
            string token = _accountService.GenerateJwtToken(dto);
            return Ok(token);
        }
    }
}
