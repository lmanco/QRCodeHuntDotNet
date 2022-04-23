using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeHuntDotNet.API.Util
{
    public interface IQRCodeImageGenerator
    {
        void WriteQRCodeToPNG(string fileName, string textToEncode, Bitmap centerImage = null);
        void WriteQRCodeToPNG(string fileName, string textToEncode, string centerText);
    }

    public class QRCodeImageGenerator : IQRCodeImageGenerator
    {
        private const int PixelsPerModule = 4;
        private const int PixelsPerModuleImageSizeScale = 45;
        private readonly int IconTextSquareSizeInPixels = PixelsPerModule * PixelsPerModuleImageSizeScale;
        private const int IconSizePercent = 25;

        private readonly IFileWriter _fileWriter;
        private readonly IFileReader _fileReader;
        private readonly IGraphicsHelper _graphicsHelper;

        public QRCodeImageGenerator(IFileWriter fileWriter, IFileReader fileReader,
            IGraphicsHelper graphicsHelper)
        {
            _fileWriter = fileWriter;
            _fileReader = fileReader;
            _graphicsHelper = graphicsHelper;
        }

        public void WriteQRCodeToPNG(string fileName, string textToEncode, Bitmap icon = null)
        {
            if (!_fileReader.FileDirectoryExists(fileName))
                _fileWriter.CreateFileDirectory(fileName);
            QRCodeData qrCodeData = new QRCodeGenerator().CreateQrCode(textToEncode, QRCodeGenerator.ECCLevel.Q);
            Bitmap qrCodeImage = new QRCode(qrCodeData).GetGraphic(PixelsPerModule, Color.Black, Color.White, icon, IconSizePercent);
            qrCodeImage.Save($"{fileName}.{ImageFormat.Png.ToString().ToLower()}", ImageFormat.Png);
        }

        public void WriteQRCodeToPNG(string fileName, string textToEncode, string iconText)
        {
            Bitmap icon = _graphicsHelper.GetTextBitmapSquare(iconText, IconTextSquareSizeInPixels);
            WriteQRCodeToPNG(fileName, textToEncode, icon);
        }
    }
}
