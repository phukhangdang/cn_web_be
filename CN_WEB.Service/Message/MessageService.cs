using CN_WEB.Core.Repository;
using CN_WEB.Core.Service;
using CN_WEB.Model.Message;
using CN_WEB.Repository.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageEntity = CN_WEB.Core.Model.Message;

namespace CN_WEB.Service.Message
{
    public class MessageService : BaseService, IMessageService
    {
        #region Private variables

        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        public MessageService(IMessageRepository messageRepository, IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Count(MessageRequestDto request)
        {
            return await _messageRepository.Count(request);
        }

        public async Task<IQueryable<MessageEntity>> Select(MessageRequestDto request)
        {
            return await _messageRepository.Select(request);
        }

        public async Task<MessageDto> SelectByID(string id)
        {
            return await _messageRepository.SelectById(id);
        }

        public async Task<MessageDto> Merge(MessageDto dto)
        {
            return await _messageRepository.Merge(dto);
        }

        public async Task<bool> DeleteById(string id)
        {
            return await _messageRepository.DeleteById(id);
        }
    }
}
