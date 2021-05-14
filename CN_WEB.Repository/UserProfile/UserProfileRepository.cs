using CN_WEB.Core.Repository;
using CN_WEB.Model.UserProfile;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfileEntity = CN_WEB.Core.Model.UserProfile;

namespace CN_WEB.Repository.UserProfile
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor

        public UserProfileRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructor

        #region Public methods
        public async Task<int> Count(UserProfileRequestDto request)
        {
            IQueryable<UserProfileEntity> query = _unitOfWork.Select<UserProfileEntity>().AsNoTracking();
            query = Filter(query, request);
            return await query.CountAsync();
        }

        public async Task<UserProfileDto> Merge(UserProfileDto model)
        {
            var result = _unitOfWork.Merge<UserProfileEntity, UserProfileDto>(model);
            return await Task.FromResult(result);
        }

        public async Task<IQueryable<UserProfileEntity>> Select(UserProfileRequestDto request)
        {
            IQueryable<UserProfileEntity> query = _unitOfWork.Select<UserProfileEntity>().AsNoTracking();
            query = Filter(query, request).OrderBy(x => x.Id);
            query = query.Paging(request);
            return await Task.FromResult(query);
        }

        public async Task<UserProfileDto> SelectById(string id)
        {
            IQueryable<UserProfileEntity> query = _unitOfWork.Select<UserProfileEntity>().AsNoTracking();
            var result = query
                .Where(x => x.Id == id)
                .Select(x => new UserProfileDto(x))
                .SingleOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            _unitOfWork.Delete<UserProfileEntity>(id);
            return await Task.FromResult(true);
        }

        #endregion

        #region Private methods

        private IQueryable<UserProfileEntity> Filter(IQueryable<UserProfileEntity> models, UserProfileRequestDto searchEntity)
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
