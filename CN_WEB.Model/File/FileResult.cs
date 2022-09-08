using System.IO;

namespace CN_WEB.Model.File
{
    public class FileResult
    {
        public FileResult()
        {
        }

        public FileResult(MemoryStream memory, string type, string name) {
            Memory = memory;
            Type = type;
            Name = name;
        }

        public MemoryStream Memory { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
