using System;
using System.Collections.Generic;
using System.Text;

namespace CN_WEB.Model.Account
{
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
