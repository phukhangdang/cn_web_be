using CN_WEB.Core.Repository;
using CN_WEB.Model.Post;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserEntity = CN_WEB.Core.Model.User;
using PostEntity = CN_WEB.Core.Model.Post;
using PostCommentEntity = CN_WEB.Core.Model.PostComment;
using PostLikeEntity = CN_WEB.Core.Model.PostLike;
using FollowedEntity = CN_WEB.Core.Model.Followed;
using CN_WEB.Repository.PostComment;
using CN_WEB.Repository.PostLike;
using CN_WEB.Model.PostComment;

namespace CN_WEB.Repository.Post
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IPostCommentRepository _postCommentRepository;

        #region Constructor

        public PostRepository(IUnitOfWork unitOfWork, IPostLikeRepository postLikeRepository, IPostCommentRepository postCommentRepository)
        {
            _unitOfWork = unitOfWork;
            _postLikeRepository = postLikeRepository;
            _postCommentRepository = postCommentRepository;
        }

        #endregion Constructor

        #region Public methods
        public async Task<int> Count(PostRequestDto request)
        {
            IQueryable<PostEntity> query = _unitOfWork.Select<PostEntity>().AsNoTracking();
            query = Filter(query, request);
            return await query.CountAsync();
        }

        public async Task<PostDto> Merge(PostDto model)
        {
            model.UserId = _unitOfWork.GetCurrentUserId();
            model.Status = 1;
            var result = _unitOfWork.Merge<PostEntity, PostDto>(model);
            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<PostDto>> Select(PostRequestDto request)
        {
            IQueryable<PostEntity> query = _unitOfWork.Select<PostEntity>().AsNoTracking();
            query = Filter(query, request).OrderByDescending(x => x.CreatedAt);
            query = query.Paging(request);
            var user = _unitOfWork.Select<UserEntity>().AsNoTracking();
            var currentUserId = _unitOfWork.GetCurrentUserId();

            var postQuery = query.Select(x => new PostDto(x)).ToList();

            IQueryable<PostCommentEntity> commentQuery = _unitOfWork.Select<PostCommentEntity>().AsNoTracking();
            IQueryable<PostLikeEntity> likeQuery = _unitOfWork.Select<PostLikeEntity>().AsNoTracking();
            var followedQuery = _unitOfWork.Select<FollowedEntity>().AsNoTracking().Where(x => x.UserId == currentUserId).ToList();
            var commentDto = commentQuery.OrderBy(x => x.CreatedAt).Select(x => new PostCommentDto(x)).ToList();

            var results = new List<PostDto>();
            foreach (var followed in followedQuery)
            {
                foreach (var post in postQuery)
                {
                    if (post.UserId == followed.FollowedId) { results.Add(post); }
                }
            }

            foreach (var comment in commentDto)
            {
                if (comment.UserId != null)
                {
                    var userId = user.Where(c => c.Id == comment.UserId).FirstOrDefault();
                    if (userId != null) { comment.UserName = userId.UserName; }
                }
            }

            foreach (var result in results)
            {
                if (result.UserId != null)
                {
                    var userId = user.Where(c => c.Id == result.UserId).FirstOrDefault();
                    if (userId != null) { result.UserName = userId.UserName; }
                }
                var temp = commentDto.Where(c => c.PostId == result.Id).ToList();
                if (temp.Count() > 0)
                {
                    result.Comments = new List<PostCommentDto>();
                    result.Comments.AddRange(temp);
                }
                var temp2 = likeQuery.Where(c => c.PostId == result.Id);
                if (temp2.Where(c => c.UserId == _unitOfWork.GetCurrentUserId()).Count() > 0) { result.Liked = true; }
                else { result.Liked = false; }
                result.CountLike = temp2.Count();
            }
            results = results.OrderByDescending(x => x.CreatedAt).ToList();
            return await Task.FromResult(results);
        }

        public async Task<PostDto> SelectById(string id)
        {
            IQueryable<PostEntity> query = _unitOfWork.Select<PostEntity>().AsNoTracking();
            var result = query
                .Where(x => x.Id == id)
                .Select(x => new PostDto(x))
                .SingleOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            _unitOfWork.Delete<PostEntity>(id);
            return await Task.FromResult(true);
        }

        #endregion

        #region Private methods

        private IQueryable<PostEntity> Filter(IQueryable<PostEntity> models, PostRequestDto searchEntity)
        {
            if (!string.IsNullOrEmpty(searchEntity.Id))
            {
                models = models.Where(x => x.Id == searchEntity.Id);
            }

            if (!string.IsNullOrEmpty(searchEntity.UserId))
            {
                models = models.Where(x => x.UserId == searchEntity.UserId);
            }

            return models;
        }

        #endregion Private methods
    }
}
