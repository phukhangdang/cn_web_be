using CN_WEB.Core.Service;
using CN_WEB.Model.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostEntity = CN_WEB.Core.Model.Post;

namespace CN_WEB.Repository.Post
{
    public interface IPostRepository : IScoped
    {
        Task<PostDto> SelectById(string id);
        Task<IEnumerable<PostDto>> Select(PostRequestDto request);
        Task<int> Count(PostRequestDto request);
        Task<PostDto> Merge(PostDto model);
        Task<bool> DeleteById(string id);
    }
}
