using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using UserEntity = CN_WEB.Core.Model.User;

namespace CN_WEB.Model.User
{
    public class UserDto : BaseModel
    {
        public UserDto(UserEntity entity) : base(entity)
        {
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int Status { get; set; }
        public int PostCount { get; set; }
        public int FollowerCount { get; set; }
        public int FollowedCount { get; set; }
        public bool IsFollowed { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
