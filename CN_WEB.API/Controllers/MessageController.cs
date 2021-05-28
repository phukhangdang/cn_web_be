using CN_WEB.Core.Model;
using CN_WEB.Model.Message;
using CN_WEB.Service.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CN_WEB.API.Controllers
{
    [Route("message")]
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<MessageDto> SelectById([FromRoute] string id)
        {
            return await _messageService.SelectByID(id);
        }

        [Route("count")]
        [HttpGet]
        public async Task<int> Count([FromQuery] MessageRequestDto request)
        {
            return await _messageService.Count(request);
        }

        [HttpGet]
        public async Task<IEnumerable<Message>> Select([FromQuery] MessageRequestDto request)
        {
            return await _messageService.Select(request);
        }

        [Route("merge")]
        [HttpPost]
        public async Task<MessageDto> Merge([FromBody] MessageDto dto)
        {
            return await _messageService.Merge(dto);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await _messageService.DeleteById(id);
        }
    }
}
