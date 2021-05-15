using CN_WEB.Core.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using UserModel = CN_WEB.Core.Model.User;

namespace CN_WEB.Core.API.Authentication
{
    public class AuthService : IAuthService
    {
        #region Private variables

        private readonly SysDbWriteContext _dbContext;
        private readonly IConfiguration _configuration;

        #endregion Private variables

        public AuthService(SysDbWriteContext dbContext, IConfiguration configuration) : base()
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public bool ValidateToken(string idToken)
        {
            try
            {
                // Read token for getting user details
                var jwtAppSettingOptions = _configuration.GetSection("JwtIssuerOptions");
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://localhost:44353",
                    ValidAudience = "http://localhost:44353",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAppSettingOptions["JwtKey"]))
                };

                // Validate token
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(idToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public UserModel GetUserFromIdToken(string idToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var idTokenInfo = tokenHandler.ReadToken(idToken) as JwtSecurityToken;
                string upn = idTokenInfo.Claims.Where(c => c.Type == "sub").Select(c => c.Value).FirstOrDefault();
                return _dbContext.User.Where(x => x.Id == upn).SingleOrDefault();
            }
            catch
            {
                return null;
            }

        }
    }
}
