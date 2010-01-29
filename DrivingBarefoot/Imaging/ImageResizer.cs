using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DrivingBarefoot.Imaging
{
    public class ImageResizer
    {
        public void MakeThumbnail(String file, String newFilePath, int thumbnailMaxWidth, int thumbnailMaxHeight)
        {
            Image bitmap = Bitmap.FromFile(file);

            if ((bitmap.Width > thumbnailMaxWidth) || (bitmap.Height > thumbnailMaxHeight))
            {
                Image resized = ResizeImage(bitmap, new Size(thumbnailMaxWidth, thumbnailMaxHeight));
                bitmap.Dispose();
                resized.Save(newFilePath);
                resized.Dispose();
            }
            else
            {
                bitmap.Dispose();
            }
        }

        public void ResizeFile(String path, int maxWidth, int maxHeight)
        {
            Image bitmap = Bitmap.FromFile(path);

            if ((bitmap.Width > maxWidth) || (bitmap.Height > maxHeight))
            {
                Image resized = ResizeImage(bitmap, new Size(maxWidth, maxHeight));
                bitmap.Dispose();
                resized.Save(path);
                resized.Dispose();
            }
            else
            {
                bitmap.Dispose();
            }
        }

        private Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
    }
}
