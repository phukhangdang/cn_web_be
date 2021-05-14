using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using MessageEntity = CN_WEB.Core.Model.Message;

namespace CN_WEB.Model.Message
{
    public class MessageDto : BaseModel
    {
        public MessageDto(MessageEntity entity) : base(entity)
        {
        }
        public string Id { get; set; }
        public string UserSendId { get; set; }
        public string UserReceive { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
