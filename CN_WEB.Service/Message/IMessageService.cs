using CN_WEB.Core.Service;
using CN_WEB.Model.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageEntity = CN_WEB.Core.Model.Message;

namespace CN_WEB.Service.Message
{
    public interface IMessageService : IScoped
    {
        Task<MessageDto> SelectByID(string id);
        Task<IQueryable<MessageEntity>> Select(MessageRequestDto request);
        Task<int> Count(MessageRequestDto request);
        Task<MessageDto> Merge(MessageDto dto);
        Task<bool> DeleteById(string id);
    }
}
