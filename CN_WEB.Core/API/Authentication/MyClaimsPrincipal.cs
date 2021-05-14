using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using UserModel = CN_WEB.Core.Model.User;

namespace CN_WEB.Core.API.Authentication
{
    public class MyClaimsPrincipal : ClaimsPrincipal
    {
        public MyClaimsPrincipal(UserModel user = null)
        {
            Id = user?.Id;
            Account = user?.Email;
        }

        public string Id { get; set; }
        public string Account { get; set; }
    }
}
