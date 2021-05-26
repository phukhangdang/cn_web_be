using CN_WEB.Core.Repository;
using CN_WEB.Core.Service;
using CN_WEB.Model.PostComment;
using CN_WEB.Repository.PostComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostCommentEntity = CN_WEB.Core.Model.PostComment;

namespace CN_WEB.Service.PostComment
{
    public class PostCommentService : BaseService, IPostCommentService
    {
        #region Private variables

        private readonly IPostCommentRepository _postCommentRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        public PostCommentService(IPostCommentRepository postCommentRepository, IUnitOfWork unitOfWork)
        {
            _postCommentRepository = postCommentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Count(PostCommentRequestDto request)
        {
            return await _postCommentRepository.Count(request);
        }

        public async Task<IEnumerable<PostCommentDto>> Select(PostCommentRequestDto request)
        {
            return await _postCommentRepository.Select(request);
        }

        public async Task<PostCommentDto> SelectByID(string id)
        {
            return await _postCommentRepository.SelectById(id);
        }

        public async Task<PostCommentDto> Merge(PostCommentDto dto)
        {
            return await _postCommentRepository.Merge(dto);
        }

        public async Task<bool> DeleteById(string id)
        {
            return await _postCommentRepository.DeleteById(id);
        }
    }
}
