using Microsoft.Extensions.Configuration;
using QRCodeHuntDotNet.API.DAL.Models;
using QRCodeHuntDotNet.API.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeHuntDotNet.API.DAL.Repositories
{
    public interface ICodeRepository
    {
        void CreateList(string gameName, IEnumerable<Code> codes);
        IEnumerable<Code> GetCodes(string gameName);
        bool CodesDataFileExists(string gameName);
    }

    public class CodeRepository : ICodeRepository
    {
        private const string DefaultDataDir = "C:\\QRCodeHuntDotNetData";
        private const string CodesFileName = "Codes.json";
        private readonly IJSONReader<IEnumerable<Code>> _jsonReader;
        private readonly IJSONWriter<IEnumerable<Code>> _jsoNWriter;
        private readonly string _dataDir;

        public CodeRepository(IJSONReader<IEnumerable<Code>> jsonReader, IJSONWriter<IEnumerable<Code>> jsoNWriter,
            IConfiguration configuration)
        {
            _jsonReader = jsonReader;
            _jsoNWriter = jsoNWriter;
            _dataDir = configuration["FileDataDirectory"] ?? DefaultDataDir;
        }

        public void CreateList(string gameName, IEnumerable<Code> codes)
        {
            _jsoNWriter.WriteToJSON(GetCodesDataFile(gameName), codes);
        }

        public IEnumerable<Code> GetCodes(string gameName)
        {
            string fileName = GetCodesDataFile(gameName);
            if (!_jsonReader.JSONFileExists(fileName))
                return null;
            return _jsonReader.ReadFromJSON(fileName);
        }

        public bool CodesDataFileExists(string gameName)
        {
            return _jsonReader.JSONFileExists(GetCodesDataFile(gameName));
        }

        private string GetCodesDataFile(string gameName)
        {
            return $"{Path.Combine(_dataDir, gameName, CodesFileName)}";
        }
    }
}
