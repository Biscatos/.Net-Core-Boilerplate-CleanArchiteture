using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Core.Application;
using Core.Application.Interfaces.Services.System;

namespace Infrastruture.Shared.Services
{
    public class LocalFilesService : ILocalFilesService
    {
        private readonly string DEFAULT_PATH = @"Files";
        public bool Delete(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            try
            {

                if (file == null || file?.Length == 0)
                    throw new Exception("File not selected");

                if (!Directory.Exists(DEFAULT_PATH))
                    Directory.CreateDirectory(DEFAULT_PATH);

                var fileName = file.FileName.GenerateUnique();
                var path = Path.Combine(DEFAULT_PATH, fileName);

                using (var fs = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> SaveAsync(string fileName, string Base64)
        {
            try
            {
                var bytes = Convert.FromBase64String(Base64);
                var ms = new MemoryStream(bytes);

                if (!Directory.Exists(DEFAULT_PATH))
                    Directory.CreateDirectory(DEFAULT_PATH);

                fileName = fileName.GenerateUnique();

                var path = Path.Combine(DEFAULT_PATH, fileName);

                using (var fs = new FileStream(path, FileMode.Create))
                {

                    await ms.CopyToAsync(fs);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> SaveTempAsync(IFormFile file)
        {
            try
            {

                var fileName = Path.Combine(Path.GetTempPath(), file.FileName.GenerateUnique());
                using (var ms = new FileStream(fileName, FileMode.Create))
                {
                    await file.CopyToAsync(ms);
                }
                return fileName;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
