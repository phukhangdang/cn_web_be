using CN_WEB.Core.Repository;
using CN_WEB.Core.Service;
using CN_WEB.Model.Followed;
using CN_WEB.Repository.Followed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FollowedEntity = CN_WEB.Core.Model.Followed;

namespace CN_WEB.Service.Followed
{
    public class FollowedService : BaseService, IFollowedService
    {
        #region Private variables

        private readonly IFollowedRepository _followedRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        public FollowedService(IFollowedRepository followedRepository, IUnitOfWork unitOfWork)
        {
            _followedRepository = followedRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Count(FollowedRequestDto request)
        {
            return await _followedRepository.Count(request);
        }

        public async Task<IQueryable<FollowedEntity>> Select(FollowedRequestDto request)
        {
            return await _followedRepository.Select(request);
        }

        public async Task<FollowedDto> SelectByID(string id)
        {
            return await _followedRepository.SelectById(id);
        }

        public async Task<FollowedDto> Merge(FollowedDto dto)
        {
            return await _followedRepository.Merge(dto);
        }

        public async Task<bool> DeleteById(string id)
        {
            return await _followedRepository.DeleteById(id);
        }
    }
}
