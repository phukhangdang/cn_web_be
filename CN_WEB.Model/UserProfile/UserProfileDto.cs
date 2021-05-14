using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using UserProfileEntity = CN_WEB.Core.Model.UserProfile;

namespace CN_WEB.Model.UserProfile
{
    public class UserProfileDto : BaseModel
    {
        public UserProfileDto(UserProfileEntity entity) : base(entity)
        {
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
