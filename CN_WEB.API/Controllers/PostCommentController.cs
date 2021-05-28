using CN_WEB.Core.Model;
using CN_WEB.Model.PostComment;
using CN_WEB.Service.PostComment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CN_WEB.API.Controllers
{
    [Route("post-comment")]
    [Authorize]
    public class PostCommentController : Controller
    {
        private readonly IPostCommentService _postCommentService;

        public PostCommentController(IPostCommentService postCommentService)
        {
            _postCommentService = postCommentService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<PostCommentDto> SelectById([FromRoute] string id)
        {
            return await _postCommentService.SelectByID(id);
        }

        [Route("count")]
        [HttpGet]
        public async Task<int> Count([FromQuery] PostCommentRequestDto request)
        {
            return await _postCommentService.Count(request);
        }

        [HttpGet]
        public async Task<IEnumerable<PostCommentDto>> Select([FromQuery] PostCommentRequestDto request)
        {
            return await _postCommentService.Select(request);
        }

        [Route("merge")]
        [HttpPost]
        public async Task<PostCommentDto> Merge([FromBody] PostCommentDto dto)
        {
            return await _postCommentService.Merge(dto);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await _postCommentService.DeleteById(id);
        }
    }
}
