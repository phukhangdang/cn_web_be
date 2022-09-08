using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CN_WEB.Core.Service;
using CN_WEB.Model.File;
using FileEntity = CN_WEB.Core.Model.File;

namespace CN_WEB.Repository.File
{
    public interface IFileRepository : IScoped
    {
        Task<FileEntity> Upload(IFormFile file, string module, string note);
        Task<FileResult> Download(string id);
        Task<bool> Delete(string id);
    }
}
