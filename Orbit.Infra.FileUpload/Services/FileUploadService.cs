using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Orbit.Infra.FileUpload.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http.Headers;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Orbit.Infra.FileUpload.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IHostingEnvironment _env;
        private readonly string _targetPath;
        private readonly string _supportedFileTypes;
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(IHostingEnvironment env, ILogger<FileUploadService> logger)
        {
            _env = env;
            _logger = logger;

            var config = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            _targetPath = config["FILE_UPLOAD_ROOT_PATH"];
            _supportedFileTypes = config["FILE_UPLOAD_SUPPORTED_TYPES"];
        }

        public Task<bool> Delete(string id)
        {
            try
            {
                var di = new DirectoryInfo(_targetPath);
                foreach (FileInfo file in di.GetFiles())
                {
                    if (file.Name.Contains(id))
                    {
                        file.Delete();
                        return Task.FromResult(true);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete File with Id {0}\nException:\n{1}",id,ex.Message);
            }
            return Task.FromResult(false);
        }

        public Task<bool> DeleteAll()
        {
            try
            {
                var di = new DirectoryInfo(_targetPath);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete all Files\nException:\n{1}", ex.Message);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<string>> GetAll()
        {
            try
            {
                if (!Directory.Exists(_targetPath))
                {
                    Directory.CreateDirectory(_targetPath);
                }
                var files = Directory.GetFiles(_targetPath).Select(e => Path.GetFileName(e));
                return Task.FromResult(files);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to retreive a List of all Files\nException:\n{1}", ex.Message);
            }
            return null;
        }

        public async Task<bool> Upload(IFormFile file)
        {
            try
            {
                if (!Directory.Exists(_targetPath))
                {
                    Directory.CreateDirectory(_targetPath);
                }
                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    string fileType = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Split(new char[] { '.' }).LastOrDefault();
                    fileType = fileType.Replace("\"", "");
                    if (fileType == null) return false;
                    if (IsTypeValid(fileType))
                    {
                        string fullPath = Path.Combine(_targetPath, fileName + "." + fileType);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Failed to upload File\nException:\n{1}", ex.Message);
            }
            
            return false;
        }

        private bool IsTypeValid(string type)
        {
            return _supportedFileTypes.Contains(type);
        }
    }
}
