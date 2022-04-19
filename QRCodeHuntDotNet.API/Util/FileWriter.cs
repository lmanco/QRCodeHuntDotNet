using System.IO;

namespace QRCodeHuntDotNet.API.Util
{
    public interface IFileWriter
    {
        void WriteAllText(string fileName, string contents);
        void DeleteFile(string fileName);
        void CreateFileDirectory(string fileName);
    }

    public class FileWriter : IFileWriter
    {
#pragma warning disable CA1822 // Mark members as static
        public void WriteAllText(string fileName, string contents)
#pragma warning restore CA1822 // Mark members as static
        {
            File.WriteAllText(fileName, contents);
        }

#pragma warning disable CA1822 // Mark members as static
        public void DeleteFile(string fileName)
#pragma warning restore CA1822 // Mark members as static
        {
            File.Delete(fileName);
        }

#pragma warning disable CA1822 // Mark members as static
        public void CreateFileDirectory(string fileName)
#pragma warning restore CA1822 // Mark members as static
        {
            Directory.CreateDirectory(new FileInfo(fileName).DirectoryName);
        }
    }
}
