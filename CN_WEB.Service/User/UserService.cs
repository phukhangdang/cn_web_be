using CN_WEB.Core.Repository;
using CN_WEB.Core.Service;
using CN_WEB.Model.User;
using CN_WEB.Repository.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserEntity = CN_WEB.Core.Model.User;

namespace CN_WEB.Service.User
{
    public class UserService : BaseService, IUserService
    {
        #region Private variables

        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Count(UserRequestDto request)
        {
            return await _userRepository.Count(request);
        }

        public async Task<IQueryable<UserEntity>> Select(UserRequestDto request)
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
    }
}
