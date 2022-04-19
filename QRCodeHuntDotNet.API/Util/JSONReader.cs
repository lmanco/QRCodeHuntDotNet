using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeHuntDotNet.API.Util
{
    public interface IJSONReader<T>
    {
        T ReadFromJSON(string fileName);
        bool JSONFileExists(string fileName);
    }

    public class JSONReader<T> : IJSONReader<T>
    {
        public const string JSONFileExtension = ".json";
        private readonly IFileReader _fileReader;

        public JSONReader(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public T ReadFromJSON(string fileName)
        {
            return JsonConvert.DeserializeObject<T>(_fileReader.ReadAllText(fileName));
        }

        public bool JSONFileExists(string fileName)
        {
            if (_fileReader.GetFileExtension(fileName) != JSONFileExtension)
                fileName += JSONFileExtension;
            return _fileReader.FileExists(fileName);
        }
    }
}
