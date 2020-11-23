using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using OpenCvSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FacesApi.Controllers
{
    [Route("api/faces")]
    [ApiController]
    public class FacesController : ControllerBase
    {
        private readonly AzureFaceConfiguration _configuration;

        public FacesController(AzureFaceConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<Tuple<List<byte[]>,Guid>> ReadFaces(Guid orderId)
        {
            List<byte[]> facesCropped = null;

            using (var ms = new MemoryStream(2048))
            {
                await Request.Body.CopyToAsync(ms);
                var bytes = ms.ToArray();
                Image img = Image.Load(bytes);
                img.Save("dummy.jpg");
                facesCropped = await UploadAnddetectFaces(img, new MemoryStream(bytes));

                //var faces = GetFaces(ms.ToArray());

                return new Tuple<List<byte[]>, Guid>(facesCropped, orderId);
            }
        }

        public static IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) {Endpoint = endpoint };
        }

        private async Task<List<byte[]>> UploadAnddetectFaces(Image img, MemoryStream imageStream)
        {
            string endPoint = _configuration.AzureEndPoint;
            string subscriptionKey = _configuration.AzureSubscriptionKey;

           var client =  Authenticate(endPoint, subscriptionKey);
            var faceList = new List<byte[]>();

            IList<DetectedFace> faces = null;

            try
            {
                faces = await client.Face.DetectWithStreamAsync(imageStream, true, false, null);

                int j = 0;

                foreach(var face in faces)
                {
                    var s = new MemoryStream();
                    var zoom = 1.0;

                    int h = (int)(face.FaceRectangle.Height / zoom);
                    int w = (int)(face.FaceRectangle.Width / zoom);
                    int x = face.FaceRectangle.Left;
                    int y = face.FaceRectangle.Top;

                    img.Clone(ctx => ctx.Crop(new Rectangle(x, y, w, h))).Save("face" + j + ".jpg");
                    img.Clone(ctx => ctx.Crop(new Rectangle(x, y, w, h))).SaveAsJpeg(s);

                    faceList.Add(s.ToArray());

                    j++;
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                throw;
            }

            return faceList;
        }

        //private List<byte[]> GetFaces(byte[] image)
        //{
        //    //convert image(in byte array) to Math type object
        //    Mat src = Cv2.ImDecode(image, ImreadModes.Color);

        //    // Save image in the root for testing
        //    src.SaveImage("image.jpg", new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));

        //    var file = Path.Combine(Directory.GetCurrentDirectory(), "CascadeFile", "haarcascade_frontalface_default.xml");

        //    var faceCascade = new CascadeClassifier();
        //    faceCascade.Load(file);

        //    var faces = faceCascade.DetectMultiScale(src, 1.1, 6, HaarDetectionType.DoRoughSearch, new Size(60, 60));

        //    var faceList = new List<byte[]>();
        //    var j = 0;
        //    foreach (var rect in faces)
        //    {
        //        var faceImage = new Mat(src, rect);
        //        faceList.Add(faceImage.ToBytes(".jpg"));
        //        faceImage.SaveImage("face" + j + ".jpg", new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));
        //        j++;
        //    }
        //    return faceList;
        //}


    }
}
