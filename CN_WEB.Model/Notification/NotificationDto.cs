using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using NotificationEntity = CN_WEB.Core.Model.Notification;

namespace CN_WEB.Model.Notification
{
    public class NotificationDto : BaseModel
    {
        public NotificationDto(NotificationEntity entity) : base(entity)
        {
        }
        public string Id { get; set; }
        public string UserSendId { get; set; }
        public string UserSendName { get; set; }
        public string UserReceive { get; set; }
        public string PostId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
