using CN_WEB.Core.Model;
using CN_WEB.Model.Notification;
using CN_WEB.Service.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CN_WEB.API.Controllers
{
    [Route("notification")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<NotificationDto> SelectById([FromRoute] string id)
        {
            return await _notificationService.SelectByID(id);
        }

        [Route("count")]
        [HttpGet]
        public async Task<int> Count([FromQuery] NotificationRequestDto request)
        {
            return await _notificationService.Count(request);
        }

        [HttpGet]
        public async Task<IEnumerable<NotificationDto>> Select([FromQuery] NotificationRequestDto request)
        {
            return await _notificationService.Select(request);
        }

        [Route("merge")]
        [HttpPost]
        public async Task<NotificationDto> Merge([FromBody] NotificationDto dto)
        {
            return await _notificationService.Merge(dto);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await _notificationService.DeleteById(id);
        }
    }
}
