using CN_WEB.Core.Repository;
using CN_WEB.Model.Message;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageEntity = CN_WEB.Core.Model.Message;

namespace CN_WEB.Repository.Message
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor

        public MessageRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructor

        #region Public methods
        public async Task<int> Count(MessageRequestDto request)
        {
            IQueryable<MessageEntity> query = _unitOfWork.Select<MessageEntity>().AsNoTracking();
            query = Filter(query, request);
            return await query.CountAsync();
        }

        public async Task<MessageDto> Merge(MessageDto model)
        {
            var result = _unitOfWork.Merge<MessageEntity, MessageDto>(model);
            return await Task.FromResult(result);
        }

        public async Task<IQueryable<MessageEntity>> Select(MessageRequestDto request)
        {
            IQueryable<MessageEntity> query = _unitOfWork.Select<MessageEntity>().AsNoTracking();
            query = Filter(query, request).OrderBy(x => x.Id);
            query = query.Paging(request);
            return await Task.FromResult(query);
        }

        public async Task<MessageDto> SelectById(string id)
        {
            IQueryable<MessageEntity> query = _unitOfWork.Select<MessageEntity>().AsNoTracking();
            var result = query
                .Where(x => x.Id == id)
                .Select(x => new MessageDto(x))
                .SingleOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            _unitOfWork.Delete<MessageEntity>(id);
            return await Task.FromResult(true);
        }

        #endregion

        #region Private methods

        private IQueryable<MessageEntity> Filter(IQueryable<MessageEntity> models, MessageRequestDto searchEntity)
        {
            if (!string.IsNullOrEmpty(searchEntity.Id))
            {
                models = models.Where(x => x.Id == searchEntity.Id);
            }

            return models;
        }

        #endregion Private methods
    }
}
