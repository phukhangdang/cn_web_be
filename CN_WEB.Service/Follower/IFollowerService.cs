using CN_WEB.Core.Service;
using CN_WEB.Model.Follower;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FollowerEntity = CN_WEB.Core.Model.Follower;

namespace CN_WEB.Service.Follower
{
    public interface IFollowerService : IScoped
    {
        Task<FollowerDto> SelectByID(string id);
        Task<IQueryable<FollowerEntity>> Select(FollowerRequestDto request);
        Task<int> Count(FollowerRequestDto request);
        Task<FollowerDto> Merge(FollowerDto dto);
        Task<bool> DeleteById(string id);
    }
}
