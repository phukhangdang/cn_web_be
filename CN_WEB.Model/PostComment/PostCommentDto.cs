using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using PostCommentEntity = CN_WEB.Core.Model.PostComment;

namespace CN_WEB.Model.PostComment
{
    public class PostCommentDto : BaseModel
    {
        public PostCommentDto(PostCommentEntity entity) : base(entity)
        {
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PostId { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
