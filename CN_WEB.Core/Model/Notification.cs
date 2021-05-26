using System;
using System.Collections.Generic;

namespace CN_WEB.Core.Model
{
    public partial class Notification
    {
        public string Id { get; set; }
        public string UserSendId { get; set; }
        public string UserReceive { get; set; }
        public string PostId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
