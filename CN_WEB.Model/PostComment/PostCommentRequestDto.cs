using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CN_WEB.Model.PostComment
{
    public class PostCommentRequestDto : BaseRequestDto
    {
        public string Id { get; set; }
        public string PostId { get; set; }
    }
}
