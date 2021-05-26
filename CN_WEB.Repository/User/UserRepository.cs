using CN_WEB.Core.Repository;
using CN_WEB.Core.Utility;
using CN_WEB.Model.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserEntity = CN_WEB.Core.Model.User;
using FollowerEntity = CN_WEB.Core.Model.Follower;
using FollowedEntity = CN_WEB.Core.Model.Followed;
using PostEntity = CN_WEB.Core.Model.Post;

namespace CN_WEB.Repository.User
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        #region Constructor

        public UserRepository(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _config = configuration;
        }

        #endregion Constructor

        #region Public methods
        public async Task<int> Count(UserRequestSelectDto request)
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

        public async Task<IEnumerable<UserDto>> Select(UserRequestSelectDto request)
        {
            IQueryable<UserEntity> query = _unitOfWork.Select<UserEntity>().AsNoTracking();
            query = Filter(query, request).OrderBy(x => x.Id);
            query = query.Paging(request);
            var dataResult = query.Select(x => new UserDto(x)).ToList();

            var follower = _unitOfWork.Select<FollowerEntity>();
            var followed = _unitOfWork.Select<FollowedEntity>();
            var post = _unitOfWork.Select<PostEntity>();
            var userId = _unitOfWork.GetCurrentUserId();

            foreach (var result in dataResult)
            {
                if (follower.Where(x => x.UserId == result.Id).Where(x => x.FollowerId == userId).ToList().Count() > 0) { result.IsFollowed = true; }
                else { result.IsFollowed = false; }
                result.FollowerCount = follower.Where(x => x.UserId == result.Id).Count();
                result.FollowedCount = followed.Where(x => x.UserId == result.Id).Count();
                result.PostCount = post.Where(x => x.UserId == result.Id).Count();
            }
            dataResult = dataResult.OrderByDescending(x => x.FollowerCount).ThenByDescending(x => x.CreatedAt).ToList();

            return await Task.FromResult(dataResult);
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

        public async Task<UserEntity> Update(UserDto model)
        {
            _unitOfWork.Update<UserEntity, UserDto>(model);
            return await _unitOfWork.FindAsync<UserEntity>(model.Id);
        }

        public async Task<UserEntity> Update(UserEntity entity)
        {
            _unitOfWork.Update(entity);
            return await _unitOfWork.FindAsync<UserEntity>(entity.Id);
        }

        public async Task<UserEntity> Create(UserEntity model)
        {
            _unitOfWork.Insert(model);
            return await _unitOfWork.FindAsync<UserEntity>(model.Id);
        }

        public async Task<bool> ResetPassDefault(string userId)
        {
            IQueryable<UserEntity> query = _unitOfWork.Select<UserEntity>().AsNoTracking();
            var user = query.Where(x => x.Id == userId).SingleOrDefault();
            if (user != null)
            {
                user.Password = PassExtension.HashPassword(_config["Setting:PassworDefault"]);
                _unitOfWork.Update(user);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        #endregion

        #region Private methods

        private IQueryable<UserEntity> Filter(IQueryable<UserEntity> models, UserRequestSelectDto searchEntity)
        {
            if (!string.IsNullOrEmpty(searchEntity.Id))
            {
                models = models.Where(x => x.Id == searchEntity.Id);
            }

            if (!string.IsNullOrEmpty(searchEntity.UserName))
            {
                models = models.Where(x => x.UserName.Contains(searchEntity.UserName));
            }

            return models;
        }

        #endregion Private methods
    }
}
