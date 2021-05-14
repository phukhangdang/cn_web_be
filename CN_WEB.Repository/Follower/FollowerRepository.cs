using CN_WEB.Core.Repository;
using CN_WEB.Model.Follower;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FollowerEntity = CN_WEB.Core.Model.Follower;

namespace CN_WEB.Repository.Follower
{
    public class FollowerRepository : BaseRepository, IFollowerRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor

        public FollowerRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructor

        #region Public methods
        public async Task<int> Count(FollowerRequestDto request)
        {
            IQueryable<FollowerEntity> query = _unitOfWork.Select<FollowerEntity>().AsNoTracking();
            query = Filter(query, request);
            return await query.CountAsync();
        }

        public async Task<FollowerDto> Merge(FollowerDto model)
        {
            var result = _unitOfWork.Merge<FollowerEntity, FollowerDto>(model);
            return await Task.FromResult(result);
        }

        public async Task<IQueryable<FollowerEntity>> Select(FollowerRequestDto request)
        {
            IQueryable<FollowerEntity> query = _unitOfWork.Select<FollowerEntity>().AsNoTracking();
            query = Filter(query, request).OrderBy(x => x.Id);
            query = query.Paging(request);
            return await Task.FromResult(query);
        }

        public async Task<FollowerDto> SelectById(string id)
        {
            IQueryable<FollowerEntity> query = _unitOfWork.Select<FollowerEntity>().AsNoTracking();
            var result = query
                .Where(x => x.Id == id)
                .Select(x => new FollowerDto(x))
                .SingleOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            _unitOfWork.Delete<FollowerEntity>(id);
            return await Task.FromResult(true);
        }

        #endregion

        #region Private methods

        private IQueryable<FollowerEntity> Filter(IQueryable<FollowerEntity> models, FollowerRequestDto searchEntity)
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
