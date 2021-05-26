using CN_WEB.Core.Repository;
using CN_WEB.Model.PostComment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostCommentEntity = CN_WEB.Core.Model.PostComment;
using UserEntity = CN_WEB.Core.Model.User;

namespace CN_WEB.Repository.PostComment
{
    public class PostCommentRepository : BaseRepository, IPostCommentRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor

        public PostCommentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructor

        #region Public methods
        public async Task<int> Count(PostCommentRequestDto request)
        {
            IQueryable<PostCommentEntity> query = _unitOfWork.Select<PostCommentEntity>().AsNoTracking();
            query = Filter(query, request);
            return await query.CountAsync();
        }

        public async Task<PostCommentDto> Merge(PostCommentDto model)
        {
            model.UserId = _unitOfWork.GetCurrentUserId();
            var result = _unitOfWork.Merge<PostCommentEntity, PostCommentDto>(model);
            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<PostCommentDto>> Select(PostCommentRequestDto request)
        {
            IQueryable<PostCommentEntity> query = _unitOfWork.Select<PostCommentEntity>().AsNoTracking();
            query = Filter(query, request).OrderBy(x => x.Id);
            query = query.Paging(request);

            var results = query.Select(x => new PostCommentDto(x)).ToList();

            var user = _unitOfWork.Select<UserEntity>().AsNoTracking();
            foreach (var result in results)
            {
                if (result.UserId != null)
                {
                    var userId = user.Where(c => c.Id == result.UserId).FirstOrDefault();
                    if (userId != null) { result.UserName = userId.UserName; }
                }
            }
            return await Task.FromResult(results);
        }

        public async Task<PostCommentDto> SelectById(string id)
        {
            IQueryable<PostCommentEntity> query = _unitOfWork.Select<PostCommentEntity>().AsNoTracking();
            var result = query
                .Where(x => x.Id == id)
                .Select(x => new PostCommentDto(x))
                .SingleOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            _unitOfWork.Delete<PostCommentEntity>(id);
            return await Task.FromResult(true);
        }

        #endregion

        #region Private methods

        private IQueryable<PostCommentEntity> Filter(IQueryable<PostCommentEntity> models, PostCommentRequestDto searchEntity)
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
