using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace IRO_UNMO.Util.FileManager
{
    public class FileManager : IFileManager
    {
        private readonly string _webRootPath;

        public FileManager(IHostingEnvironment hostingEnvironment)
        {
            _webRootPath = hostingEnvironment.WebRootPath;
        }

        public async Task<string> Save(IFormFile file, string path, string fileName = null, bool createDestinationPath = true)
        {
            if (file == null || string.IsNullOrWhiteSpace(path))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(fileName))
                fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var relativePath = Path.Combine(path, fileName);
            var absolutePath = Path.Combine(_webRootPath, relativePath.TrimStart('/')).Replace("/", @"\");

            var destinationPath = Path.Combine(_webRootPath, path.TrimStart('/')).Replace("/", @"\");
            if (createDestinationPath && !Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            try
            {
                using (var fileStream = new FileStream(absolutePath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    await file.CopyToAsync(fileStream);

                    return relativePath.Replace("\\","/");
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Move(string relativeSourcePath, string relativeDestinationPath, string fileName = null, bool createDestinationPath = true)
        {
            relativeDestinationPath = Path.GetDirectoryName(relativeDestinationPath);

            var absoluteSourcePath = Path.Combine(_webRootPath, relativeSourcePath.TrimStart('/'));
            var absoluteDestinationPath = Path.Combine(_webRootPath, relativeDestinationPath.TrimStart('\\'));

            if (!File.Exists(absoluteSourcePath))
                return string.Empty;

            if (File.Exists(absoluteDestinationPath))
                return string.Empty;

            if (createDestinationPath && !Directory.Exists(absoluteDestinationPath))
                Directory.CreateDirectory(absoluteDestinationPath);

            if (string.IsNullOrWhiteSpace(fileName))
                fileName = Path.GetFileName(absoluteSourcePath);

            try
            {
                File.Move(absoluteSourcePath, Path.Combine(absoluteDestinationPath, fileName));

                return Path.Combine(relativeDestinationPath, fileName).Replace('\\', '/');
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool Exists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            var filePath = Path.Combine(_webRootPath, path.TrimStart('/'));
            return File.Exists(filePath);
        }

        public bool Exists(string path, string fileName)
        {
            return Exists(Path.Combine(path, fileName));
        }

        public bool Delete(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            var filePath = Path.Combine(_webRootPath, path.TrimStart('/'));
            if (!File.Exists(filePath))
                return false;

            try
            {
                File.Delete(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(string path, string fileName)
        {
            return Delete(Path.Combine(path, fileName));
        }
    }
}