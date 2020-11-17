using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace FacesApiTest
{
    public class ImageUtility
    {
        public byte[] ConvertToBytes(string imagePath)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (var filestream = new FileStream(imagePath, FileMode.Open))
            {
                filestream.CopyTo(memoryStream);
            }

            return memoryStream.ToArray();
        }

        public void FromBytesToImage(byte[] imageBytes, string fileName)
        {
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                //convert a memory stream object into an image type
                Image image = Image.FromStream(memoryStream);
                image.Save(fileName + ".jpg", ImageFormat.Jpeg);
            }
        }
    }
}
