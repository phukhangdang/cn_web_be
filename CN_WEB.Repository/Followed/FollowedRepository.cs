using CN_WEB.Core.Repository;
using CN_WEB.Model.Followed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FollowedEntity = CN_WEB.Core.Model.Followed;

namespace CN_WEB.Repository.Followed
{
    public class FollowedRepository : BaseRepository, IFollowedRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor

        public FollowedRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructor

        #region Public methods
        public async Task<int> Count(FollowedRequestDto request)
        {
            IQueryable<FollowedEntity> query = _unitOfWork.Select<FollowedEntity>().AsNoTracking();
            query = Filter(query, request);
            return await query.CountAsync();
        }

        public async Task<FollowedDto> Merge(FollowedDto model)
        {
            var result = _unitOfWork.Merge<FollowedEntity, FollowedDto>(model);
            return await Task.FromResult(result);
        }

        public async Task<IQueryable<FollowedEntity>> Select(FollowedRequestDto request)
        {
            IQueryable<FollowedEntity> query = _unitOfWork.Select<FollowedEntity>().AsNoTracking();
            query = Filter(query, request).OrderBy(x => x.Id);
            query = query.Paging(request);
            return await Task.FromResult(query);
        }

        public async Task<FollowedDto> SelectById(string id)
        {
            IQueryable<FollowedEntity> query = _unitOfWork.Select<FollowedEntity>().AsNoTracking();
            var result = query
                .Where(x => x.Id == id)
                .Select(x => new FollowedDto(x))
                .SingleOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            _unitOfWork.Delete<FollowedEntity>(id);
            return await Task.FromResult(true);
        }

        #endregion

        #region Private methods

        private IQueryable<FollowedEntity> Filter(IQueryable<FollowedEntity> models, FollowedRequestDto searchEntity)
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
