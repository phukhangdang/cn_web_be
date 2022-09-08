using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CN_WEB.Core.Service;
using CN_WEB.Model.File;
using CN_WEB.Repository.File;
using FileEntity = CN_WEB.Core.Model.File;

namespace CN_WEB.Service.File
{
    public class FileService : BaseService, IFileService
    {
        private readonly IFileRepository _repository;

        public FileService(IFileRepository repository)
        {
            _repository = repository;
        }

        public async Task<FileResult> Download(string id)
        {
            return await _repository.Download(id);
        }

        public async Task<bool> Delete(string id)
        {
            return await _repository.Delete(id);
        }

        public async Task<List<FileEntity>> Upload(FileRequestDto request, IFormFileCollection files)
        {
            List<FileEntity> result = new List<FileEntity>();
            if(files != null)
            {
                foreach (var file in files)
                {
                    FileEntity fileInfo = await _repository.Upload(file, request.Module, request.Note);
                    result.Add(fileInfo);
                }
            }

            return result;
        }
    }
}
