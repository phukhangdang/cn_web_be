using CN_WEB.Core.Repository;
using CN_WEB.Model.PostLike;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostLikeEntity = CN_WEB.Core.Model.PostLike;

namespace CN_WEB.Repository.PostLike
{
    public class PostLikeRepository : BaseRepository, IPostLikeRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor

        public PostLikeRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructor

        #region Public methods
        public async Task<int> Count(PostLikeRequestDto request)
        {
            IQueryable<PostLikeEntity> query = _unitOfWork.Select<PostLikeEntity>().AsNoTracking();
            query = Filter(query, request);
            return await query.CountAsync();
        }

        public async Task<PostLikeDto> Merge(PostLikeDto model)
        {
            model.UserId = _unitOfWork.GetCurrentUserId();
            var result = _unitOfWork.Merge<PostLikeEntity, PostLikeDto>(model);
            return await Task.FromResult(result);
        }

        public async Task<IQueryable<PostLikeEntity>> Select(PostLikeRequestDto request)
        {
            IQueryable<PostLikeEntity> query = _unitOfWork.Select<PostLikeEntity>().AsNoTracking();
            query = Filter(query, request).OrderBy(x => x.Id);
            query = query.Paging(request);
            return await Task.FromResult(query);
        }

        public async Task<PostLikeDto> SelectById(string id)
        {
            IQueryable<PostLikeEntity> query = _unitOfWork.Select<PostLikeEntity>().AsNoTracking();
            var result = query
                .Where(x => x.Id == id)
                .Select(x => new PostLikeDto(x))
                .SingleOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            _unitOfWork.Delete<PostLikeEntity>(id);
            return await Task.FromResult(true);
        }

        #endregion

        #region Private methods

        private IQueryable<PostLikeEntity> Filter(IQueryable<PostLikeEntity> models, PostLikeRequestDto searchEntity)
        {
            if (!string.IsNullOrEmpty(searchEntity.Id))
            {
                models = models.Where(x => x.Id == searchEntity.Id);
            }

            if (!string.IsNullOrEmpty(searchEntity.PostId))
            {
                models = models.Where(x => x.PostId == searchEntity.PostId);
            }

            return models;
        }

        #endregion Private methods
    }
}
