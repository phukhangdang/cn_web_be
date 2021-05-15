using CN_WEB.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModel = CN_WEB.Core.Model.User;

namespace CN_WEB.Repository.Account
{
    public interface IAccountRepository : IScoped
    {
        Task<IQueryable<UserModel>> Select();
    }
}
