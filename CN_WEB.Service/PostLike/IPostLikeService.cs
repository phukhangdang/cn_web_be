using CN_WEB.Core.Service;
using CN_WEB.Model.PostLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostLikeEntity = CN_WEB.Core.Model.PostLike;

namespace CN_WEB.Service.PostLike
{
    public interface IPostLikeService : IScoped
    {
        Task<PostLikeDto> SelectByID(string id);
        Task<IQueryable<PostLikeEntity>> Select(PostLikeRequestDto request);
        Task<int> Count(PostLikeRequestDto request);
        Task<PostLikeDto> Merge(PostLikeDto dto);
        Task<bool> DeleteById(string id);
    }
}
