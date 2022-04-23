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
        void CreateList(string gameName, IEnumerable<Code> codes, string siteUrl);
        IEnumerable<Code> GetCodes(string gameName);
        bool CodesDataFileExists(string gameName);
    }

    public class CodeRepository : ICodeRepository
    {
        private const string DefaultDataDir = "C:\\QRCodeHuntDotNetData";
        private const string CodesFileName = "Codes.json";
        private const string CodeImagesDirectoryName = "code_images";
        private readonly IJSONReader<IEnumerable<Code>> _jsonReader;
        private readonly IJSONWriter<IEnumerable<Code>> _jsoNWriter;
        private readonly IQRCodeImageGenerator _qRCodeImageGenerator;
        private readonly string _dataDir;

        public CodeRepository(IJSONReader<IEnumerable<Code>> jsonReader, IJSONWriter<IEnumerable<Code>> jsoNWriter,
            IQRCodeImageGenerator qRCodeImageGenerator, IConfiguration configuration)
        {
            _jsonReader = jsonReader;
            _jsoNWriter = jsoNWriter;
            _qRCodeImageGenerator = qRCodeImageGenerator;
            _dataDir = configuration["FileDataDirectory"] ?? DefaultDataDir;
        }

        public void CreateList(string gameName, IEnumerable<Code> codes, string siteUrl)
        {
            _jsoNWriter.WriteToJSON(GetCodesDataFile(gameName), codes);
            string codeImagesDirectory = Path.Combine(_dataDir, gameName, CodeImagesDirectoryName);
            foreach (Code code in codes)
            {
                string codeImageFile = Path.Combine(codeImagesDirectory, code.Key.ToString());
                string codeImageLink = $"{siteUrl}/{code.Key}";
                _qRCodeImageGenerator.WriteQRCodeToPNG(codeImageFile, codeImageLink, $"{code.Num}");
            }
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
            return Path.Combine(_dataDir, gameName, CodesFileName);
        }
    }
}
