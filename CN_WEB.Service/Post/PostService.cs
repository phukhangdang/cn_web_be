using CN_WEB.Core.Repository;
using CN_WEB.Core.Service;
using CN_WEB.Model.Post;
using CN_WEB.Repository.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostEntity = CN_WEB.Core.Model.Post;

namespace CN_WEB.Service.Post
{
    public class PostService : BaseService, IPostService
    {
        #region Private variables

        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Count(PostRequestDto request)
        {
            return await _postRepository.Count(request);
        }

        public async Task<IEnumerable<PostDto>> Select(PostRequestDto request)
        {
            return await _postRepository.Select(request);
        }

        public async Task<PostDto> SelectByID(string id)
        {
            return await _postRepository.SelectById(id);
        }

        public async Task<PostDto> Merge(PostDto dto)
        {
            return await _postRepository.Merge(dto);
        }

        public async Task<bool> DeleteById(string id)
        {
            return await _postRepository.DeleteById(id);
        }
    }
}
