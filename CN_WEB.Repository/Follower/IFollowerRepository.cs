using CN_WEB.Core.Service;
using CN_WEB.Model.Follower;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FollowerEntity = CN_WEB.Core.Model.Follower;

namespace CN_WEB.Repository.Follower
{
    public interface IFollowerRepository : IScoped
    {
        Task<FollowerDto> SelectById(string id);
        Task<IQueryable<FollowerEntity>> Select(FollowerRequestDto request);
        Task<int> Count(FollowerRequestDto request);
        Task<FollowerDto> Merge(FollowerDto model);
        Task<bool> DeleteById(string id);
    }
}
