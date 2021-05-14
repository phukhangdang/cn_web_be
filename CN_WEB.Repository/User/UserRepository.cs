using CN_WEB.Core.Repository;
using CN_WEB.Model.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserEntity = CN_WEB.Core.Model.User;

namespace CN_WEB.Repository.User
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor

        public UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructor

        #region Public methods
        public async Task<int> Count(UserRequestDto request)
        {
            IQueryable<UserEntity> query = _unitOfWork.Select<UserEntity>().AsNoTracking();
            query = Filter(query, request);
            return await query.CountAsync();
        }

        public async Task<UserDto> Merge(UserDto model)
        {
            var result = _unitOfWork.Merge<UserEntity, UserDto>(model);
            return await Task.FromResult(result);
        }

        public async Task<IQueryable<UserEntity>> Select(UserRequestDto request)
        {
            IQueryable<UserEntity> query = _unitOfWork.Select<UserEntity>().AsNoTracking();
            query = Filter(query, request).OrderBy(x => x.Id);
            query = query.Paging(request);
            return await Task.FromResult(query);
        }

        public async Task<UserDto> SelectById(string id)
        {
            IQueryable<UserEntity> query = _unitOfWork.Select<UserEntity>().AsNoTracking();
            var result = query
                .Where(x => x.Id == id)
                .Select(x => new UserDto(x))
                .SingleOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            _unitOfWork.Delete<UserEntity>(id);
            return await Task.FromResult(true);
        }

        #endregion

        #region Private methods

        private IQueryable<UserEntity> Filter(IQueryable<UserEntity> models, UserRequestDto searchEntity)
        {
            if (!string.IsNullOrEmpty(searchEntity.Id))
            {
                models = models.Where(x => x.Id == searchEntity.Id);
            }

            return models;
        }

        #endregion Private methods
    }
}
