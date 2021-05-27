using CN_WEB.Core.Repository;
using CN_WEB.Model.Notification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationEntity = CN_WEB.Core.Model.Notification;
using UserEntity = CN_WEB.Core.Model.User;

namespace CN_WEB.Repository.Notification
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor

        public NotificationRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructor

        #region Public methods
        public async Task<int> Count(NotificationRequestDto request)
        {
            IQueryable<NotificationEntity> query = _unitOfWork.Select<NotificationEntity>().AsNoTracking();
            query = Filter(query, request);
            return await query.CountAsync();
        }

        public async Task<NotificationDto> Merge(NotificationDto model)
        {
            model.UserSendId = _unitOfWork.GetCurrentUserId();
            var result = _unitOfWork.Merge<NotificationEntity, NotificationDto>(model);
            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<NotificationDto>> Select(NotificationRequestDto request)
        {
            IQueryable<NotificationEntity> query = _unitOfWork.Select<NotificationEntity>().AsNoTracking();
            query = Filter(query, request).OrderByDescending(x => x.CreatedAt);
            query = query.Paging(request);
            var user = _unitOfWork.Select<UserEntity>().AsNoTracking();

            var results = query.Select(x => new NotificationDto(x)).ToList();

            foreach (var result in results)
            {
                if (result.UserSendId != null)
                {
                    var userId = user.Where(c => c.Id == result.UserSendId).FirstOrDefault();
                    if (userId != null) { result.UserSendName = userId.UserName; }
                }
            }

            return await Task.FromResult(results);
        }

        public async Task<NotificationDto> SelectById(string id)
        {
            IQueryable<NotificationEntity> query = _unitOfWork.Select<NotificationEntity>().AsNoTracking();
            var result = query
                .Where(x => x.Id == id)
                .Select(x => new NotificationDto(x))
                .SingleOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteById(string id)
        {
            _unitOfWork.Delete<NotificationEntity>(id);
            return await Task.FromResult(true);
        }

        #endregion

        #region Private methods

        private IQueryable<NotificationEntity> Filter(IQueryable<NotificationEntity> models, NotificationRequestDto searchEntity)
        {
            if (!string.IsNullOrEmpty(searchEntity.Id))
            {
                models = models.Where(x => x.Id == searchEntity.Id);
            }

            if (!string.IsNullOrEmpty(searchEntity.UserSendId))
            {
                models = models.Where(x => x.UserSendId == searchEntity.UserSendId);
            }

            return models;
        }

        #endregion Private methods
    }
}
