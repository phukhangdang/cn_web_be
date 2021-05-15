using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CN_WEB.Model.User
{
    public class UserRequestSelectDto : BaseRequestDto
    {
        public string Id { get; set; }
        public short Status { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
