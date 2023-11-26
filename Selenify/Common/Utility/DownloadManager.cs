using OpenQA.Selenium;
using Selenify.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Selenify.Common.Helpers;
using System.IO;
using static Selenify.Common.Utility.WebDriverManager;
using System.Net;

namespace Selenify.Common.Utility
{
    public static class DownloadManager
    {
        public static async void DownloadFileWithCookiesAndProgressBarAsync(string fileUrl, string path, string progressPrefix)
        {
            ConsoleProgressBar progressBar = new ConsoleProgressBar();
            var handler = new HttpClientHandler();
            handler.CookieContainer = CreateCookieContainerFromWebDriverCookies();
            using (HttpClient client = new HttpClient(handler))
            {
                client.Timeout = TimeSpan.FromMinutes(5);

                string savePath;
                using (Stream stream = client.GetAsync(fileUrl).Result.Content.ReadAsStream())
                {
                    savePath = GetFilePathForDownload(fileUrl, path, stream);
                }

                using (var file = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await HttpClientExtensions.DownloadAsync(client, fileUrl, file, progress, new CancellationToken());
                }
            }

        }

        public static async void DownloadFileWithProgressBarAsync(string fileUrl, string path, string progressPrefix = "")
        {
            var progress = new ConsoleProgressBar(progressPrefix);
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);

                string savePath;
                using (var stream = HttpClientHelper.GetAsync(fileUrl).Result.Content.ReadAsStream())
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

        public static async void DownloadFileWithCookiesAsync(string fileUrl, string path)
        {
            var handler = new HttpClientHandler();
            handler.CookieContainer = CreateCookieContainerFromWebDriverCookies();
            using(HttpClient tempClient = new HttpClient(handler))
            {
                HttpResponseMessage response = await tempClient.GetAsync(fileUrl);
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

        private static CookieContainer CreateCookieContainerFromWebDriverCookies()
        {
            var cookies = Driver.Manage().Cookies.AllCookies;
            CookieContainer cookieContainer = new CookieContainer();
            foreach (OpenQA.Selenium.Cookie cookie in cookies)
            {
                cookieContainer.Add(
                    new System.Net.Cookie(
                        cookie.Name,
                        cookie.Value,
                        cookie.Path,
                        cookie.Domain));
            }

            return cookieContainer;
        }
    }
}
