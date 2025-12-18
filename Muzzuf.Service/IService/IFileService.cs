using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Muzzuf.Service.IService
{
    public interface IFileService
    {

        Task<string> UploadAsync(IFormFile file, string folderName);
        void Delete(string fileUrl);
    }
}
