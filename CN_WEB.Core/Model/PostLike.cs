﻿using System;
using System.Collections.Generic;

namespace CN_WEB.Core.Model
{
    public partial class PostLike
    {
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
