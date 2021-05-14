using CN_WEB.Core.Service;
using CN_WEB.Model.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfileEntity = CN_WEB.Core.Model.UserProfile;

namespace CN_WEB.Repository.UserProfile
{
    public interface IUserProfileRepository : IScoped
    {
        Task<UserProfileDto> SelectById(string id);
        Task<IQueryable<UserProfileEntity>> Select(UserProfileRequestDto request);
        Task<int> Count(UserProfileRequestDto request);
        Task<UserProfileDto> Merge(UserProfileDto model);
        Task<bool> DeleteById(string id);
    }
}
