using CN_WEB.Core.Service;
using CN_WEB.Model.PostComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostCommentEntity = CN_WEB.Core.Model.PostComment;

namespace CN_WEB.Service.PostComment
{
    public interface IPostCommentService : IScoped
    {
        Task<PostCommentDto> SelectByID(string id);
        Task<IEnumerable<PostCommentDto>> Select(PostCommentRequestDto request);
        Task<int> Count(PostCommentRequestDto request);
        Task<PostCommentDto> Merge(PostCommentDto dto);
        Task<bool> DeleteById(string id);
    }
}
