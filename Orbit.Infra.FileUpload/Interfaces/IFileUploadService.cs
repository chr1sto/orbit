using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Infra.FileUpload.Interfaces
{
    public interface IFileUploadService
    {
        Task<IEnumerable<string>> GetAll();
        Task<bool> DeleteAll();
        Task<bool> Delete(string id);
        Task<bool> Upload(IFormFile file);
    }
}
