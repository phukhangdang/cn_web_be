using CN_WEB.Core.Repository;
using CN_WEB.Core.Service;
using CN_WEB.Core.Utility;
using CN_WEB.Model.User;
using CN_WEB.Model.UserProfile;
using CN_WEB.Repository.User;
using CN_WEB.Repository.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserEntity = CN_WEB.Core.Model.User;
using FollowerEntity = CN_WEB.Core.Model.Follower;
using FollowedEntity = CN_WEB.Core.Model.Followed;
using Microsoft.EntityFrameworkCore;
using CN_WEB.Core.API;

namespace CN_WEB.Service.User
{
    public class UserService : BaseService, IUserService
    {
        #region Private variables

        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        public UserService(IUserRepository userRepository, IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Count(UserRequestSelectDto request)
        {
            return await _userRepository.Count(request);
        }

        public async Task<IEnumerable<UserDto>> Select(UserRequestSelectDto request)
        {
            return await _userRepository.Select(request);
        }

        public async Task<UserDto> SelectByID(string id)
        {
            return await _userRepository.SelectById(id);
        }

        public async Task<UserDto> Merge(UserDto dto)
        {
            return await _userRepository.Merge(dto);
        }

        public async Task<bool> DeleteById(string id)
        {
            return await _userRepository.DeleteById(id);
        }

        public async Task<UserDto> Create(UserRegisterDto user)
        {
            // Begin transaction
            using var transaction = _unitOfWork.BeginTransaction();

            // Check trung user
            var allUser = _unitOfWork.Select<UserEntity>().AsNoTracking();
            if (allUser.Where(x => x.UserName == user.UserName).Count() > 0)
            {
                throw new BadRequestException("User has been already in D2B2LM!");
            }
            else if (allUser.Where(x => x.Email == user.Email).Count() > 0)
            {
                throw new BadRequestException("User has been already in D2B2LM!");
            }

            // Merge user
            var userInfo = new UserEntity { Id = Guid.NewGuid().ToString("N") };
            userInfo.UserName = user.UserName;
            userInfo.Email = user.Email;
            userInfo.Role = user.Role;
            userInfo.Status = user.Status;
            userInfo.Password = PassExtension.HashPassword(user.Password);
            await _userRepository.Create(userInfo);

            // Merge user profile
            var userProfileInfo = new UserProfileDto();
            userProfileInfo.UserId = userInfo.Id;
            userProfileInfo.FirstName = user.FirstName;
            userProfileInfo.LastName = user.LastName;
            userProfileInfo.PhoneNumber = user.PhoneNumber;
            await _userProfileRepository.Merge(userProfileInfo);

            // Merge follow
            var follower = new FollowerEntity();
            follower.UserId = userInfo.Id;
            follower.FollowerId = userInfo.Id;
            var followed = new FollowedEntity();
            followed.UserId = userInfo.Id;
            followed.FollowedId = userInfo.Id;
            _unitOfWork.Merge(follower);
            _unitOfWork.Merge(followed);

            // Commit transaction
            transaction.Commit();
            return await _userRepository.SelectById(userInfo.Id);
        }

        public async Task<bool> ResetPassDefault(string id)
        {
            return await _userRepository.ResetPassDefault(id);
        }

    }
}
