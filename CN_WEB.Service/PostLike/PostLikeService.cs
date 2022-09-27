using CN_WEB.Core.Repository;
using CN_WEB.Core.Service;
using CN_WEB.Model.PostLike;
using CN_WEB.Repository.PostLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostLikeEntity = CN_WEB.Core.Model.PostLike;

namespace CN_WEB.Service.PostLike
{
    public class PostLikeService : BaseService, IPostLikeService
    {
        #region Private variables

        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        public PostLikeService(IPostLikeRepository postLikeRepository, IUnitOfWork unitOfWork)
        {
            _postLikeRepository = postLikeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Count(PostLikeRequestDto request)
        {
            return await _postLikeRepository.Count(request);
        }

        public async Task<IQueryable<PostLikeEntity>> Select(PostLikeRequestDto request)
        {
            return await _postLikeRepository.Select(request);
        }

        public async Task<PostLikeDto> SelectByID(string id)
        {
            return await _postLikeRepository.SelectById(id);
        }

        public async Task<PostLikeDto> Merge(PostLikeDto dto)
        {
            return await _postLikeRepository.Merge(dto);
        }

        public async Task<bool> DeleteById(string id)
        {
            return await _postLikeRepository.DeleteById(id);
        }
    }
}

/*
Cách đóng gói của sản phẩm này cũng
không mấy lạ lẫm nữa vì tôi đã từng gặp
ở phiên bản V12: sắp xếp rất gọn gàng trong
chiếc hộp hình chữ nhật, hay thậm chí có thể
nói là đội ngũ Dyson đã đóng gói rất tài
tình đến nỗi khi tôi lấy tất cả những đầu
hút ra hết thì không tài nào đặt vào vừa vặn được nữa.
 */