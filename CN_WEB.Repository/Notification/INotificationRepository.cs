using CN_WEB.Core.Service;
using CN_WEB.Model.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationEntity = CN_WEB.Core.Model.Notification;

namespace CN_WEB.Repository.Notification
{
    public interface INotificationRepository : IScoped
    {
        Task<NotificationDto> SelectById(string id);
        Task<IEnumerable<NotificationDto>> Select(NotificationRequestDto request);
        Task<int> Count(NotificationRequestDto request);
        Task<NotificationDto> Merge(NotificationDto model);
        Task<bool> DeleteById(string id);
    }
}
