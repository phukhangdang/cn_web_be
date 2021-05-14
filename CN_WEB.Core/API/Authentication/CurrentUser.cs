using CN_WEB.Core.Model;

namespace CN_WEB.Core.API.Authentication
{
    public class CurrentUser : BaseModel
    {
        public string Id { get; set; }
        public string Account { get; set; }

        public CurrentUser() : base() { }
        public CurrentUser(object obj) : base(obj) { }
    }
}
