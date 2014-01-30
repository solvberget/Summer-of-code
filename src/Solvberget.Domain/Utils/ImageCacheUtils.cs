using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace Solvberget.Domain.Utils
{
    public static class ImageCacheUtils
    {
        public static void DownloadImageFromUrl(string imageUrl, string imageName, string pathToCache)
        {
            System.Drawing.Image image = null;

            if (!Directory.Exists(pathToCache))
                Directory.CreateDirectory(pathToCache);

            var fileName = Path.Combine(pathToCache, imageName);
            if (File.Exists(fileName))
                return;


            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                var webResponse = webRequest.GetResponse();

                var stream = webResponse.GetResponseStream();

                image = Image.FromStream(stream);

                webResponse.Close();

                image.Save(fileName);
            }
            catch (Exception)
            {
                //Todo add logging here
            }

        }
    }
}