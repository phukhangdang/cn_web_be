using CN_WEB.Core.Model;
using CN_WEB.Model.UserProfile;
using CN_WEB.Service.UserProfile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CN_WEB.API.Controllers
{
    [Route("user-profile")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<UserProfileDto> SelectById([FromRoute] string id)
        {
            return await _userProfileService.SelectByID(id);
        }

        [Route("count")]
        [HttpGet]
        public async Task<int> Count([FromQuery] UserProfileRequestDto request)
        {
            return await _userProfileService.Count(request);
        }

        [HttpGet]
        public async Task<IEnumerable<UserProfile>> Select([FromQuery] UserProfileRequestDto request)
        {
            return await _userProfileService.Select(request);
        }

        [Route("merge")]
        [HttpPost]
        public async Task<UserProfileDto> Merge([FromBody] UserProfileDto dto)
        {
            return await _userProfileService.Merge(dto);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await _userProfileService.DeleteById(id);
        }
    }
}
