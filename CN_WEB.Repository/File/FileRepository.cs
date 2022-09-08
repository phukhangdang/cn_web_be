using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CN_WEB.Core.Repository;
using CN_WEB.Model.File;
using FileEntity = CN_WEB.Core.Model.File;

namespace CN_WEB.Repository.File
{
    public class FileRepository : BaseRepository, IFileRepository
    {
        private string _rootPath = string.Empty;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public FileRepository(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _rootPath = Directory.GetParent(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).Parent.Parent.ToString() + _configuration["UploadLocation:MainPath"];
        }

        public async Task<FileEntity> Upload(IFormFile file, string module, string note)
        {
            var entity = new FileEntity();

            // Copy file to file server and rename with date time information
            if (file.Length > 0)
            {
                var id = Guid.NewGuid().ToString("N");

                // Get file name
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string extension = Path.GetExtension(fileName);

                var pathFolder = Path.Combine(_rootPath, module);

                // Insert file infomation to database
                entity.Id = id;
                entity.Name = fileName;
                entity.Module = module;
                entity.Extension = extension;
                entity.LocationPath = Path.Combine(pathFolder, id.ToString() + extension);
                entity.Size = file.Length;
                entity.Note = note;

                _unitOfWork.Insert(entity);

                // Check exist folder
                if (!Directory.Exists(pathFolder))
                {
                    Directory.CreateDirectory(pathFolder);
                }

                // Copy file to disk
                using var stream = new FileStream(Path.Combine(pathFolder, id.ToString() + extension), FileMode.Create);
                file.CopyTo(stream);
            }

            return await Task.FromResult(entity);
        }

        public async Task<FileResult> Download(string id)
        {
            var file = _unitOfWork.Select<FileEntity>().Where(x => x.Id == id).FirstOrDefault();
            if (file != null)
            {
                string filePath = Path.Combine(_rootPath, file.Module, file.Id + file.Extension);
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return new FileResult(memory, FileExtension.GetContentType(file.LocationPath), file.Name);
            }
            else
            {
                return new FileResult();
            }
        }

        public async Task<bool> Delete(string id)
        {
            var file = _unitOfWork.Find<FileEntity>(id);

            if (file != null)
            {
                _unitOfWork.Delete(file);

                if (System.IO.File.Exists(file.LocationPath))
                {
                    System.IO.File.Delete(file.LocationPath);
                }

                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }
    }
}