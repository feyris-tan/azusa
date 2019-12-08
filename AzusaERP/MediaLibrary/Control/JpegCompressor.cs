using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    static class JpegCompressor
    {
        public static byte[] CompressJpeg(Image i, int targetSize)
        {
            long quality = 100;
            MemoryStream ms;
            ImageCodecInfo imageCodecInfo = GetEncoder(ImageFormat.Jpeg);
            Encoder encoder = Encoder.Quality;
            EncoderParameters encoderParameters = new EncoderParameters(1);

            do
            {
                encoderParameters.Param[0] = new EncoderParameter(encoder, quality);
                ms = new MemoryStream();
                i.Save(ms, imageCodecInfo, encoderParameters);
                quality--;
            } while (ms.Position > targetSize);

            int copySize = (int)ms.Position;
            byte[] result = new byte[copySize];
            Array.Copy(ms.GetBuffer(), 0, result, 0, copySize);
            ms.Dispose();
            return result;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
