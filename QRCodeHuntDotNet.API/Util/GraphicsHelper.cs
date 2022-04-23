using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeHuntDotNet.API.Util
{
    public interface IGraphicsHelper
    {
        Bitmap GetTextBitmapSquare(string text, int size);
    }

    public class GraphicsHelper : IGraphicsHelper
    {
        private const string DefaultFont = "Tahoma";
        private const int DefaultFontSize = 64;

        public Bitmap GetTextBitmapSquare(string text, int size)
        {
            Bitmap image = new(size, size);
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, size, size));
            g.DrawString(text, new Font(DefaultFont, DefaultFontSize), Brushes.Black,
                new PointF(size / 2, size / 2), new StringFormat {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
            g.Flush();
            return image;
        }
    }
}
