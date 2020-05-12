using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace IRO_UNMO.Util.FileManager
{
    public interface IFileManager
    {
        Task<string> Save(IFormFile file, string path, string fileName = null, bool createDestinationPath = true);
        string Move(string relativeSourcePath, string relativeDestinationPath, string fileName = null, bool createDestinationPath = true);
        bool Exists(string path);
        bool Exists(string path, string fileName);
        bool Delete(string path);
        bool Delete(string path, string fileName);
    }
}