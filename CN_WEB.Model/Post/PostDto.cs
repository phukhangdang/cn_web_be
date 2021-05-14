using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using PostEntity = CN_WEB.Core.Model.Post;

namespace CN_WEB.Model.Post
{
    public class PostDto : BaseModel
    {
        public PostDto(PostEntity entity) : base(entity)
        {
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
