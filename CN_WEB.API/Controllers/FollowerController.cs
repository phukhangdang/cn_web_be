using CN_WEB.Core.Model;
using CN_WEB.Model.Follower;
using CN_WEB.Service.Follower;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CN_WEB.API.Controllers
{
    [Route("follower")]
    public class FollowerController : Controller
    {
        private readonly IFollowerService _followerService;

        public FollowerController(IFollowerService followerService)
        {
            _followerService = followerService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<FollowerDto> SelectById([FromRoute] string id)
        {
            return await _followerService.SelectByID(id);
        }

        [Route("count")]
        [HttpGet]
        public async Task<int> Count([FromQuery] FollowerRequestDto request)
        {
            return await _followerService.Count(request);
        }

        [HttpGet]
        public async Task<IEnumerable<Follower>> Select([FromQuery] FollowerRequestDto request)
        {
            return await _followerService.Select(request);
        }

        [Route("merge")]
        [HttpPost]
        public async Task<FollowerDto> Merge([FromBody] FollowerDto dto)
        {
            return await _followerService.Merge(dto);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await _followerService.DeleteById(id);
        }
    }
}
