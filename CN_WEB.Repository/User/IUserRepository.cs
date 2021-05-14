using CN_WEB.Core.Service;
using CN_WEB.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserEntity = CN_WEB.Core.Model.User;

namespace CN_WEB.Repository.User
{
    public interface IUserRepository : IScoped
    {
        Task<UserDto> SelectById(string id);
        Task<IQueryable<UserEntity>> Select(UserRequestDto request);
        Task<int> Count(UserRequestDto request);
        Task<UserDto> Merge(UserDto model);
        Task<bool> DeleteById(string id);
    }
}
