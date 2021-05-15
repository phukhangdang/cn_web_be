using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using FollowerEntity = CN_WEB.Core.Model.Follower;

namespace CN_WEB.Model.Follower
{
    public class FollowerDto : BaseModel
    {
        public FollowerDto(FollowerEntity entity) : base(entity)
        {
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FollowerId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
