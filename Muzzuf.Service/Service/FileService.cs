using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Muzzuf.Service.CustomError;
using Muzzuf.Service.IService;

namespace Muzzuf.Service.Service
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void Delete(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl)) return;

            var fullPath = Path.Combine(_env.WebRootPath, fileUrl.TrimStart('/'));
            if(File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public async Task<string> UploadAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            var uploadsPath = Path.Combine(_env.WebRootPath, folderName);
            if(!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);

            await file.CopyToAsync(stream);

            return $"/{folderName}/{fileName}";
        }
    }
}
