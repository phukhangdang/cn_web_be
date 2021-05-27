using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CN_WEB.Model.Notification
{
    public class NotificationRequestDto : BaseRequestDto
    {
        public string Id { get; set; }
        public string UserSendId { get; set; }
        public string UserReceive { get; set; }
    }
}
