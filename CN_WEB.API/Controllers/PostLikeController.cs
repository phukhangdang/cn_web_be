using CN_WEB.Core.Model;
using CN_WEB.Model.PostLike;
using CN_WEB.Service.PostLike;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CN_WEB.API.Controllers
{
    [Route("post-like")]
    public class PostLikeController : Controller
    {
        private readonly IPostLikeService _postLikeService;

        public PostLikeController(IPostLikeService postLikeService)
        {
            _postLikeService = postLikeService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<PostLikeDto> SelectById([FromRoute] string id)
        {
            return await _postLikeService.SelectByID(id);
        }

        [Route("count")]
        [HttpGet]
        public async Task<int> Count([FromQuery] PostLikeRequestDto request)
        {
            return await _postLikeService.Count(request);
        }

        [HttpGet]
        public async Task<IEnumerable<PostLike>> Select([FromQuery] PostLikeRequestDto request)
        {
            return await _postLikeService.Select(request);
        }

        [Route("merge")]
        [HttpPost]
        public async Task<PostLikeDto> Merge([FromBody] PostLikeDto dto)
        {
            return await _postLikeService.Merge(dto);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await _postLikeService.DeleteById(id);
        }
    }
}
