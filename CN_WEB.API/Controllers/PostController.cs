using CN_WEB.Core.Model;
using CN_WEB.Model.Post;
using CN_WEB.Service.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CN_WEB.API.Controllers
{
    [Route("post")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<PostDto> SelectById([FromRoute] string id)
        {
            return await _postService.SelectByID(id);
        }

        [Route("count")]
        [HttpGet]
        public async Task<int> Count([FromQuery] PostRequestDto request)
        {
            return await _postService.Count(request);
        }

        [HttpGet]
        public async Task<IEnumerable<Post>> Select([FromQuery] PostRequestDto request)
        {
            return await _postService.Select(request);
        }

        [Route("merge")]
        [HttpPost]
        public async Task<PostDto> Merge([FromBody] PostDto dto)
        {
            return await _postService.Merge(dto);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await _postService.DeleteById(id);
        }
    }
}
