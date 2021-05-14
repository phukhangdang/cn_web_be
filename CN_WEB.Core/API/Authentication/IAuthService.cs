using CN_WEB.Core.Service;
using UserModel = CN_WEB.Core.Model.User;

namespace CN_WEB.Core.API.Authentication
{
    public interface IAuthService : IScoped
    {
        bool ValidateToken(string idToken);
        UserModel GetUserFromIdToken(string idToken);
        string RenewTokenId(string idToken);
    }
}
