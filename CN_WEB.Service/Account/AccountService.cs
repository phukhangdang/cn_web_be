using CN_WEB.Core.API.Authentication;
using CN_WEB.Core.Service;
using CN_WEB.Core.Utility;
using CN_WEB.Model.Account;
using CN_WEB.Model.User;
using CN_WEB.Repository.Account;
using CN_WEB.Repository.User;
using CN_WEB.Service.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountDto = CN_WEB.Model.Account.Account;
using UserModel = CN_WEB.Core.Model.User;

namespace CN_WEB.Service.Account
{
    /// <summary>
    /// Service interface
    /// </summary>
    public interface IAccountService : IScoped
    {
        Task<AccountDto> Authenticate(HttpContext httpContext, AccountRequestDto request);
        Task<AccountChildDto> Login(LoginRequestDto request);
    }

    /// <summary>
    /// Service class
    /// </summary>
    public class AccountService : BaseService, IAccountService
    {
        #region Private variables

        private readonly IAccountRepository _accountRepository;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;


        #endregion Private variables

        #region Public methods

        public AccountService(
            IAccountRepository accountRepository,
            IConfiguration configuration,
            IAuthService authService) : base()
        {
            _accountRepository = accountRepository;
            _authService = authService;
            _configuration = configuration;
        }

        public async Task<AccountDto> Authenticate(HttpContext httpContext, AccountRequestDto request)
        {
            string idToken = httpContext.Request.Headers["id-token"];
            var result = new AccountDto();

            // Case 1: Token id is valid
            if (_authService.ValidateToken(idToken))
            {
                var userModel = _authService.GetUserFromIdToken(idToken);
                result.IsAuthenticated = true;
                result.CanAccess = true;
                result.User = new AccountInfo(userModel);
            }
            // Case 2: Token id is invalid
            else
            {
                string url = "";
                result.IsAuthenticated = false;
                result.Redirect = url;
            }

            return await Task.FromResult(result);
        }

        public async Task<AccountChildDto> Login(LoginRequestDto request)
        {
            var userInfo = new UserModel();
            if (!string.IsNullOrEmpty(request.UserName))
            {
                userInfo = (await _accountRepository.Select().ConfigureAwait(false))
                    .FirstOrDefault(x => x.UserName == request.UserName.Trim());
            }
            else if (!string.IsNullOrEmpty(request.Email))
            {
                userInfo = (await _accountRepository.Select().ConfigureAwait(false))
                    .FirstOrDefault(x => x.Email == request.Email.Trim());
            }
            var result = new AccountChildDto();
            if (userInfo != null)
            {
                var checkPass = PassExtension.VerifyPassword(userInfo.Password, request.Password);

                if (checkPass)
                {
                    var accessToken = GenerateJwtToken(userInfo);
                    if (string.IsNullOrEmpty(accessToken))
                    {
                        return null;
                    }

                    result.UserInfo = new AccountChildInfo();
                    result.UserInfo.Id = userInfo.Id;
                    result.UserInfo.UserName = userInfo.UserName;
                    result.UserInfo.Role = userInfo.Role;
                    result.AccessToken = accessToken;
                    result.CanAccess = true;
                }
            }

            return await Task.FromResult(result);
        }

        #endregion Public methods

        #region Private methods

        private string GenerateJwtToken(UserModel user)
        {
            IdentityOptions _options = new IdentityOptions();
            // establish list of claim -- list of important information, each claim contains each infomation
            // add username, id, an encode factor
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // get options
            var jwtAppSettingOptions = _configuration.GetSection("JwtIssuerOptions");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAppSettingOptions["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtAppSettingOptions["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                jwtAppSettingOptions["JwtIssuer"],
                jwtAppSettingOptions["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion Private methods
    }
}
