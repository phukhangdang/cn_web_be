using CN_WEB.Core.Repository;
using CN_WEB.Core.Service;
using CN_WEB.Model.Notification;
using CN_WEB.Repository.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationEntity = CN_WEB.Core.Model.Notification;

namespace CN_WEB.Service.Notification
{
    public class PostService : BaseService, INotificationService
    {
        #region Private variables

        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        public PostService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Count(NotificationRequestDto request)
        {
            return await _notificationRepository.Count(request);
        }

        public async Task<IEnumerable<NotificationDto>> Select(NotificationRequestDto request)
        {
            return await _notificationRepository.Select(request);
        }

        public async Task<NotificationDto> SelectByID(string id)
        {
            return await _notificationRepository.SelectById(id);
        }

        public async Task<NotificationDto> Merge(NotificationDto dto)
        {
            return await _notificationRepository.Merge(dto);
        }

        public async Task<bool> DeleteById(string id)
        {
            return await _notificationRepository.DeleteById(id);
        }
    }
}
