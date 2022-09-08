using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CN_WEB.Service.File;
using CN_WEB.Model.File;
using FileEntity = CN_WEB.Core.Model.File;
using Microsoft.AspNetCore.Hosting;

namespace CN_WEB.API.Controllers
{
    [Route("file")]
    [ApiController]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<List<FileEntity>> Upload([FromQuery] FileRequestDto request)
        {
            return await _fileService.Upload(request, Request.Form.Files);
        }

        [Route("{id}"), HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            var fileInfo = await _fileService.Download(id);
            return File(fileInfo.Memory, fileInfo.Type, fileInfo.Name);
        }

        [Route("{id}"), HttpDelete]
        public async Task<bool> Delete([FromRoute] string id)
        {
            return await _fileService.Delete(id);
        }
    }
}
