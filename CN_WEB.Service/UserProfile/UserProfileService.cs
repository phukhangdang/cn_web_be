using CN_WEB.Core.Repository;
using CN_WEB.Core.Service;
using CN_WEB.Model.UserProfile;
using CN_WEB.Repository.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfileEntity = CN_WEB.Core.Model.UserProfile;

namespace CN_WEB.Service.UserProfile
{
    public class UserProfileService : BaseService, IUserProfileService
    {
        #region Private variables

        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        public UserProfileService(IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork)
        {
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Count(UserProfileRequestDto request)
        {
            return await _userProfileRepository.Count(request);
        }

        public async Task<IQueryable<UserProfileEntity>> Select(UserProfileRequestDto request)
        {
            return await _userProfileRepository.Select(request);
        }

        public async Task<UserProfileDto> SelectByID(string id)
        {
            return await _userProfileRepository.SelectById(id);
        }

        public async Task<UserProfileDto> Merge(UserProfileDto dto)
        {
            return await _userProfileRepository.Merge(dto);
        }

        public async Task<bool> DeleteById(string id)
        {
            return await _userProfileRepository.DeleteById(id);
        }
    }
}
