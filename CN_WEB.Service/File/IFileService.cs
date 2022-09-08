using CN_WEB.Core.Service;
using CN_WEB.Model.File;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileEntity = CN_WEB.Core.Model.File;

namespace CN_WEB.Service.File
{
    public interface IFileService : IScoped
    {
        Task<List<FileEntity>> Upload(FileRequestDto request, IFormFileCollection files);
        Task<FileResult> Download(string id);
        Task<bool> Delete(string id);
    }
}
