using CN_WEB.Core.Model;

namespace CN_WEB.Model.File
{
    public class FileRequestDto : BaseRequestDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public string Note { get; set; }
        public string ModuleStartsWith { get; set; }
        public int? ModuleLevel { get; set; }
    }
}
