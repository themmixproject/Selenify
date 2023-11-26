using OpenQA.Selenium;
using Selenify.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Selenify.Common.Helpers;
using System.IO;

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

                string savePath;
                using (var stream = HttpClientHelper.GetAsync(fileUrl).Result.Content.ReadAsStreamAsync().Result)
                {
                    savePath = GetFilePathForDownload(fileUrl, path, stream);
                }

                using (var file = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await HttpClientExtensions.DownloadAsync(client, fileUrl, file, progress, new CancellationToken());
                }
            }
        }

        public static async void DownloadFileAsync(string fileUrl, string path)
        {
            HttpResponseMessage response = HttpClientHelper.Get(fileUrl);
            HttpContent responseContent = response.Content;
            using (var stream = await responseContent.ReadAsStreamAsync())
            {
                string savePath = GetFilePathForDownload(fileUrl, path, stream);
                using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    stream.Position = 0;
                    await stream.CopyToAsync(fileStream);
                }
            }
        }

        private static string GetFilePathForDownload(string fileUrl, string path, Stream fileStream)
        {
            string urlWithoutQuery = new Uri(fileUrl).GetLeftPart(UriPartial.Path);
            string fileName = FileHelper.GetFileNameFromUrlOrDefault(urlWithoutQuery);
            fileName += FileHelper.GetFileExtensionFromUrlOrStream(urlWithoutQuery, fileStream);

            string saveDirectory = Path.GetDirectoryName(path)!;
            fileName = FileHelper.IncrementFileNameIfDuplicate(saveDirectory, fileName);

            return Path.Combine(saveDirectory!, fileName);
        }


    }
}
