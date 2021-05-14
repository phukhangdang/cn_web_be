using CN_WEB.Core.Service;
using CN_WEB.Model.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageEntity = CN_WEB.Core.Model.Message;

namespace CN_WEB.Repository.Message
{
    public interface IMessageRepository : IScoped
    {
        Task<MessageDto> SelectById(string id);
        Task<IQueryable<MessageEntity>> Select(MessageRequestDto request);
        Task<int> Count(MessageRequestDto request);
        Task<MessageDto> Merge(MessageDto model);
        Task<bool> DeleteById(string id);
    }
}
