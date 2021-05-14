using CN_WEB.Core.Service;
using CN_WEB.Model.Followed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FollowedEntity = CN_WEB.Core.Model.Followed;

namespace CN_WEB.Service.Followed
{
    public interface IFollowedService : IScoped
    {
        Task<FollowedDto> SelectByID(string id);
        Task<IQueryable<FollowedEntity>> Select(FollowedRequestDto request);
        Task<int> Count(FollowedRequestDto request);
        Task<FollowedDto> Merge(FollowedDto dto);
        Task<bool> DeleteById(string id);
    }
}
