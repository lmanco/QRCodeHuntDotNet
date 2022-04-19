using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeHuntDotNet.API.Util
{
    public interface IJSONWriter<T>
    {
        void WriteToJSON(string fileName, T obj);
    }

    public class JSONWriter<T> : IJSONWriter<T>
    {
        public const string JSONFileExtension = ".json";
        private readonly IFileWriter _fileWriter;
        private readonly IFileReader _fileReader;

        public JSONWriter(IFileWriter fileWriter, IFileReader fileReader)
        {
            _fileWriter = fileWriter;
            _fileReader = fileReader;
        }

        public void WriteToJSON(string fileName, T obj)
        {
            if (!_fileReader.FileDirectoryExists(fileName))
                _fileWriter.CreateFileDirectory(fileName);
            _fileWriter.WriteAllText(fileName, JsonConvert.SerializeObject(obj));
        }
    }
}
