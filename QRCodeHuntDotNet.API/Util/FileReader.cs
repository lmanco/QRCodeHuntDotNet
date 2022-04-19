using System.IO;
using System.Linq;

namespace QRCodeHuntDotNet.API.Util
{
    public interface IFileReader
    {
        string ReadAllText(string fileName);
        byte[] ReadAllBytes(string fileName);
        string[] GetFilesInDirectory(string directoryName, string searchPattern);
        string GetFileExtension(string fileName);
        bool FileDirectoryExists(string fileName);
        bool FileExists(string fileName);
        string[] GetFileNamesWithoutPathsOrExtensions(string[] fileNames);
    }

    public class FileReader : IFileReader
    {
#pragma warning disable CA1822 // Mark members as static
        public string ReadAllText(string fileName)
#pragma warning restore CA1822 // Mark members as static
        {
            return File.ReadAllText(fileName);
        }

#pragma warning disable CA1822 // Mark members as static
        public byte[] ReadAllBytes(string fileName)
#pragma warning restore CA1822 // Mark members as static
        {
            return File.ReadAllBytes(fileName);
        }

#pragma warning disable CA1822 // Mark members as static
        public string[] GetFilesInDirectory(string directoryName, string searchPattern)
#pragma warning restore CA1822 // Mark members as static
        {
            return Directory.GetFiles(directoryName, searchPattern);
        }

#pragma warning disable CA1822 // Mark members as static
        public bool FileDirectoryExists(string fileName)
#pragma warning restore CA1822 // Mark members as static
        {
            return Directory.Exists(new FileInfo(fileName).DirectoryName);
        }

#pragma warning disable CA1822 // Mark members as static
        public string GetFileExtension(string fileName)
#pragma warning restore CA1822 // Mark members as static
        {
            return Path.GetExtension(fileName);
        }

#pragma warning disable CA1822 // Mark members as static
        public bool FileExists(string fileName)
#pragma warning restore CA1822 // Mark members as static
        {
            return File.Exists(fileName);
        }

#pragma warning disable CA1822 // Mark members as static
        public string[] GetFileNamesWithoutPathsOrExtensions(string[] fileNames)
#pragma warning restore CA1822 // Mark members as static
        {
            return fileNames.Select(fileName => Path.GetFileNameWithoutExtension(fileName)).ToArray();
        }
    }
}
