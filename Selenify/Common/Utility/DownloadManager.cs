using OpenQA.Selenium;
using Selenify.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Selenify.Common.Helpers;

namespace Selenify.Common.Utility
{
    public static class DownloadManager
    {
        public static async Task DownloadFileWithProgressBarAsync(string fileUrl, string path, string progressPrefix = "")
        {
            var progress = new ConsoleProgressBar(progressPrefix);
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                using (var file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await HttpClientExtensions.DownloadAsync(client, fileUrl, file, progress, new CancellationToken());
                }
            }
        }

        public static void DownloadFile(string fileUrl, string path)
        {
            HttpResponseMessage response = HttpClientHelper.Get(fileUrl);
            HttpContent responseContent = response.Content;
            using (var stream = responseContent.ReadAsStreamAsync().Result)
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyToAsync(fileStream);
                }
            }
        }


        //add teseract utility
    }
}
