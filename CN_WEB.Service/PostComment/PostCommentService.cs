using CN_WEB.Core.Repository;
using CN_WEB.Core.Service;
using CN_WEB.Model.PostComment;
using CN_WEB.Repository.PostComment;
using CN_WEB.Service.SignalRHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostCommentEntity = CN_WEB.Core.Model.PostComment;
using UserEntity = CN_WEB.Core.Model.User;

namespace CN_WEB.Service.PostComment
{
    public class PostCommentService : BaseService, IPostCommentService
    {
        #region Private variables

        private readonly IPostCommentRepository _postCommentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IHubContext<SignalRHubService, ISignalRHubService> _signalRHubService;

        #endregion

        public PostCommentService(IPostCommentRepository postCommentRepository,
                                    IUnitOfWork unitOfWork,
                                    IHubContext<SignalRHubService, ISignalRHubService> signalRHubService)
        {
            _postCommentRepository = postCommentRepository;
            _unitOfWork = unitOfWork;
            _signalRHubService = signalRHubService;
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
            var result = new PostCommentDto(await _postCommentRepository.Merge(dto));
            var user = _unitOfWork.Select<UserEntity>().AsNoTracking();
            var userId = user.Where(c => c.Id == result.UserId).FirstOrDefault();
            if (userId != null) { result.UserName = userId.UserName; }

            await _signalRHubService.Clients.All.BroadcastComment(result);
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            return await _postCommentRepository.DeleteById(id);
        }
    }
}
