using CN_WEB.Core.Repository;
using CN_WEB.Model.Post;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostEntity = CN_WEB.Core.Model.Post;

namespace CN_WEB.Repository.Post
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor

        public PostRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructor

        #region Public methods
        public async Task<int> Count(PostRequestDto request)
        {
            IQueryable<PostEntity> query = _unitOfWork.Select<PostEntity>().AsNoTracking();
            query = Filter(query, request);
            return await query.CountAsync();
        }

        public async Task<PostDto> Merge(PostDto model)
        {
            var result = _unitOfWork.Merge<PostEntity, PostDto>(model);
            return await Task.FromResult(result);
        }

        public async Task<IQueryable<PostEntity>> Select(PostRequestDto request)
        {
            IQueryable<PostEntity> query = _unitOfWork.Select<PostEntity>().AsNoTracking();
            query = Filter(query, request).OrderBy(x => x.Id);
            query = query.Paging(request);
            return await Task.FromResult(query);
        }

        public async Task<PostDto> SelectById(string id)
        {
            IQueryable<PostEntity> query = _unitOfWork.Select<PostEntity>().AsNoTracking();
            var result = query
                .Where(x => x.Id == id)
                .Select(x => new PostDto(x))
                .SingleOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            _unitOfWork.Delete<PostEntity>(id);
            return await Task.FromResult(true);
        }

        #endregion

        #region Private methods

        private IQueryable<PostEntity> Filter(IQueryable<PostEntity> models, PostRequestDto searchEntity)
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
