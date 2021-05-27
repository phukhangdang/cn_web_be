using CN_WEB.Core.Service;
using CN_WEB.Model.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationEntity = CN_WEB.Core.Model.Notification;

namespace CN_WEB.Service.Notification
{
    public interface INotificationService : IScoped
    {
        Task<NotificationDto> SelectByID(string id);
        Task<IEnumerable<NotificationDto>> Select(NotificationRequestDto request);
        Task<int> Count(NotificationRequestDto request);
        Task<NotificationDto> Merge(NotificationDto dto);
        Task<bool> DeleteById(string id);
    }
}
