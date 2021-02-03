using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Transfermarkt.Web.Services
{
    public interface IImagesService
    {
        string Upload(IFormFile file, string filename, bool replace = false);
    }

    public class ImagesService : IImagesService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ImagesService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string Upload(IFormFile file, string filename, bool replace = false)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string uniqueFileName = $"{DateTime.Now.Millisecond}_{filename}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
            
            if (!File.Exists(filePath))
            {
                file.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            else
            {
                if (replace)
                {
                    File.Delete(filePath);
                    file.CopyTo(new FileStream(filePath, FileMode.Create));
                }
            }

            return uniqueFileName;
        }
    }
}
