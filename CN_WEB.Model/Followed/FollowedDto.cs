using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using FollowedEntity = CN_WEB.Core.Model.Followed;

namespace CN_WEB.Model.Followed
{
    public class FollowedDto : BaseModel
    {
        public FollowedDto(FollowedEntity entity) : base(entity)
        {
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FollowedId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
