using CN_WEB.Core.Repository;
using CN_WEB.Core.Service;
using CN_WEB.Model.Follower;
using CN_WEB.Repository.Follower;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FollowerEntity = CN_WEB.Core.Model.Follower;

namespace CN_WEB.Service.Follower
{
    public class FollowerService : BaseService, IFollowerService
    {
        #region Private variables

        private readonly IFollowerRepository _followerRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        public FollowerService(IFollowerRepository followerRepository, IUnitOfWork unitOfWork)
        {
            _followerRepository = followerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Count(FollowerRequestDto request)
        {
            return await _followerRepository.Count(request);
        }

        public async Task<IQueryable<FollowerEntity>> Select(FollowerRequestDto request)
        {
            return await _followerRepository.Select(request);
        }

        public async Task<FollowerDto> SelectByID(string id)
        {
            return await _followerRepository.SelectById(id);
        }

        public async Task<FollowerDto> Merge(FollowerDto dto)
        {
            return await _followerRepository.Merge(dto);
        }

        public async Task<bool> DeleteById(string id)
        {
            return await _followerRepository.DeleteById(id);
        }
    }
}
