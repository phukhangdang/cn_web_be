using CN_WEB.Model.Account;
using CN_WEB.Service.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountDto = CN_WEB.Model.Account.Account;

namespace CN_WEB.API.Controllers
{
    // test git
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;

        public AccountController(IConfiguration configuration, IAccountService accountService)
        {
            _configuration = configuration;
            _accountService = accountService;
        }

        [Route("login-child")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<AccountChildDto> LoginChild([FromBody] LoginRequestDto request)
        {
            return await _accountService.LoginChild(request);
        }

        [Route("logout")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            var cookieOptions = new CookieOptions() { Expires = DateTime.Now.AddMonths(-1) };
            Response.Cookies.Append("id-token", string.Empty, cookieOptions);
            return RedirectPermanent(_configuration["APIAuthentication:idaClient"]);
        }

        [Route("authenticate")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<AccountDto> Authenticate([FromBody] AccountRequestDto request)
        {
            return await _accountService.Authenticate(HttpContext, request);
        }

    }
}
