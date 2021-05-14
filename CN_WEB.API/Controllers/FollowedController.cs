using CN_WEB.Core.Model;
using CN_WEB.Model.Followed;
using CN_WEB.Service.Followed;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CN_WEB.API.Controllers
{
    [Route("followed")]
    public class FollowedController : Controller
    {
        private readonly IFollowedService _followedService;

        public FollowedController(IFollowedService followedService)
        {
            _followedService = followedService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<FollowedDto> SelectById([FromRoute] string id)
        {
            return await _followedService.SelectByID(id);
        }

        [Route("count")]
        [HttpGet]
        public async Task<int> Count([FromQuery] FollowedRequestDto request)
        {
            return await _followedService.Count(request);
        }

        [HttpGet]
        public async Task<IEnumerable<Followed>> Select([FromQuery] FollowedRequestDto request)
        {
            return await _followedService.Select(request);
        }

        [Route("merge")]
        [HttpPost]
        public async Task<FollowedDto> Merge([FromBody] FollowedDto dto)
        {
            return await _followedService.Merge(dto);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await _followedService.DeleteById(id);
        }
    }
}
