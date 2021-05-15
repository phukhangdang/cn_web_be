using CN_WEB.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using UserEntity = CN_WEB.Core.Model.User;

namespace CN_WEB.Model.Account
{
    public class Account
    {
        public bool IsAuthenticated { get; set; }
        public bool CanAccess { get; set; }
        public AccountInfo User { get; set; }
        public string Redirect { get; set; }
    }

    public class AccountInfo : BaseModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public AccountInfo(UserEntity user) : base(user)
        {
        }
    }
}
