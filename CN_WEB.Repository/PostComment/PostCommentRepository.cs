using CN_WEB.Core.Repository;
using CN_WEB.Model.PostComment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostCommentEntity = CN_WEB.Core.Model.PostComment;

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
            var result = _unitOfWork.Merge<PostCommentEntity, PostCommentDto>(model);
            return await Task.FromResult(result);
        }

        public async Task<IQueryable<PostCommentEntity>> Select(PostCommentRequestDto request)
        {
            IQueryable<PostCommentEntity> query = _unitOfWork.Select<PostCommentEntity>().AsNoTracking();
            query = Filter(query, request).OrderBy(x => x.Id);
            query = query.Paging(request);
            return await Task.FromResult(query);
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

            return models;
        }

        #endregion Private methods
    }
}
