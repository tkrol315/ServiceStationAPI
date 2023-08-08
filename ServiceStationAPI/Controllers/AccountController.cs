
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult> Register([FromBody] RegisterAccountDto dto)
        {
            await _accountService.RegisterAccount(dto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginAccountDto dto)
        {
            string token = await _accountService.GenerateJwtToken(dto);
            return Ok(token);
        }

        [HttpPut("{email}")]
        [Authorize(Roles ="Manager")]
        public async Task<ActionResult> Update([FromRoute]string email, [FromBody] UpdateAccountDto dto)
        {
            await _accountService.UpdateAccount(email, dto);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> GetAccounts()
        {
            var accounts = await _accountService.GetAccounts();
            return Ok(accounts);
        }

        [HttpGet("{email}")]
        [Authorize(Roles = "Manager")]

        public async Task<ActionResult> GetAccount([FromRoute]string email)
        {
            var account = await _accountService.GetAccount(email);
            return Ok(account);
        }

        [HttpDelete("{email}")]
        [Authorize(Roles ="Manager")]
        public async Task<ActionResult> Delete([FromRoute] string email)
        {
            await _accountService.DeleteAccount(email);
            return NoContent();
        }
    }
}
