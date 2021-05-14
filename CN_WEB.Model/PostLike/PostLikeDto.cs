using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using PostLikeEntity = CN_WEB.Core.Model.PostLike;

namespace CN_WEB.Model.PostLike
{
    public class PostLikeDto : BaseModel
    {
        public PostLikeDto(PostLikeEntity entity) : base(entity)
        {
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PostId { get; set; }
        public int Type { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
