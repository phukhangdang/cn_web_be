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
        private readonly string _clientId, _clientSecret, _authority, _redirect, _discoverKey, _oauth;


        #endregion Private variables

        public AuthService(IConfiguration configuration, SysDbWriteContext dbContext) : base()
        {
            _dbContext = dbContext;
            _clientId = configuration["APIAuthentication:idaClientId"];
            _clientSecret = configuration["APIAuthentication:idaClientSecret"];
            _authority = configuration["APIAuthentication:idaAuthority"];
            _redirect = configuration["APIAuthentication:idaRedirect"];
            _discoverKey = configuration["APIAuthentication:idaADFSDiscoveryKey"];
            _oauth = configuration["APIAuthentication:idaOauth"];
        }

        public bool ValidateToken(string idToken)
        {
            try
            {
                // Get JSON web key
                using WebClient wc = new WebClient();
                string discoveryJson = wc.DownloadString(_discoverKey);
                dynamic adfsJson = JsonConvert.DeserializeObject<dynamic>(discoveryJson);
                JsonWebKey jsonWebKey = new JsonWebKey(adfsJson["keys"]?[0]?.ToString());

                // Read token for getting user details
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidIssuer = _authority,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jsonWebKey,
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
                string upn = idTokenInfo.Claims.Where(c => c.Type == "upn").Select(c => c.Value).FirstOrDefault();
                return _dbContext.User.Where(x => x.Email == upn).SingleOrDefault();
            }
            catch
            {
                return null;
            }

        }

        public string RenewTokenId(string idToken)
        {
            string result = string.Empty;
            try
            {
                // Get new token
                UserModel user = GetUserFromIdToken(idToken);
                if (user == null)
                {
                    return result;
                }
                NameValueCollection parameters = new NameValueCollection
                {
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret },
                    { "redirect_uri", _redirect },
                    { "refresh_token", user.RefreshToken },
                    { "scope", "openid" },
                    { "grant_type", "refresh_token" }
                };

                using WebClient wc = new WebClient();
                string json = Encoding.UTF8.GetString(wc.UploadValues(_oauth, parameters));
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
                result = obj["id_token"];
            }
            catch
            {
                result = string.Empty;
            }

            return result;
        }
    }
}
