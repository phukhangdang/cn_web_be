using CN_WEB.Core.Model;
using CN_WEB.Model.User;
using CN_WEB.Service.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CN_WEB.API.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<UserDto> SelectById([FromRoute] string id)
        {
            return await _userService.SelectByID(id);
        }

        [Route("count")]
        [HttpGet]
        public async Task<int> Count([FromQuery] UserRequestSelectDto request)
        {
            return await _userService.Count(request);
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> Select([FromQuery] UserRequestSelectDto request)
        {
            return await _userService.Select(request);
        }

        [Route("merge")]
        [HttpPost]
        public async Task<UserDto> Merge([FromBody] UserDto dto)
        {
            return await _userService.Merge(dto);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await _userService.DeleteById(id);
        }

        [Route("create")]
        [HttpPost]
        public async Task<UserDto> Create([FromBody] UserRegisterDto requestDto)
        {
            return await _userService.Create(requestDto);
        }

        [HttpGet]
        [Route("reset-pass/{id}")]
        public async Task<bool> ResetPassDefault([FromRoute] string id)
        {
            return await _userService.ResetPassDefault(id);
        }
    }
}
