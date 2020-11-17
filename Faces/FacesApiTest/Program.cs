using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FacesApiTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var imagePath = @"a53af7988a3c5eaa38a25a4a71ac69ad.jpg";
            var urlAddress = "http://localhost:6000/api/faces";

            ImageUtility imageUtil = new ImageUtility();
            var bytes = imageUtil.ConvertToBytes(imagePath);

            List<byte[]> faceList = null;

            var byteContent = new ByteArrayContent(bytes);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(urlAddress, byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    faceList = JsonConvert.DeserializeObject<List<byte[]>>(apiResponse);
                }
            }

            if (faceList.Count() > 0)
            {
                for (var i = 0; i < faceList.Count(); i++)
                {
                    imageUtil.FromBytesToImage(faceList[i], "face" + i);
                }
            }
        }
    }
}
