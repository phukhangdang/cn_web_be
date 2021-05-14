using CN_WEB.Core.Service;
using CN_WEB.Model.Followed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FollowedEntity = CN_WEB.Core.Model.Followed;

namespace CN_WEB.Repository.Followed
{
    public interface IFollowedRepository : IScoped
    {
        Task<FollowedDto> SelectById(string id);
        Task<IQueryable<FollowedEntity>> Select(FollowedRequestDto request);
        Task<int> Count(FollowedRequestDto request);
        Task<FollowedDto> Merge(FollowedDto model);
        Task<bool> DeleteById(string id);
    }
}
