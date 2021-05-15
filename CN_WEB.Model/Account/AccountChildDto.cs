using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using UserEntity = CN_WEB.Core.Model.User;

namespace CN_WEB.Model.Account
{
    public class AccountChildDto
    {
        public string AccessToken { get; set; }
        public bool CanAccess { get; set; }
        public AccountChildInfo UserInfo { get; set; }
        public string Redirect { get; set; }
    }
    public class AccountChildInfo : BaseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public AccountChildInfo(UserEntity user) : base(user)
        {
        }
        public AccountChildInfo()
        {
        }
    }
}
