using CN_WEB.Core.Service;
using CN_WEB.Model.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfileEntity = CN_WEB.Core.Model.UserProfile;

namespace CN_WEB.Service.UserProfile
{
    public interface IUserProfileService : IScoped
    {
        Task<UserProfileDto> SelectByID(string id);
        Task<IQueryable<UserProfileEntity>> Select(UserProfileRequestDto request);
        Task<int> Count(UserProfileRequestDto request);
        Task<UserProfileDto> Merge(UserProfileDto dto);
        Task<bool> DeleteById(string id);
    }
}
