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
                string urlWithoutQuery = new Uri(fileUrl).GetLeftPart(UriPartial.Path);
                string fileName = GetFileName(urlWithoutQuery, path);
                string fileExtension = GetFileExtension( urlWithoutQuery, path, stream );

                string saveDirectory = Path.GetDirectoryName(path)!;
                string newFileName = fileName + fileExtension;
                newFileName = SetFileNameOccurance(saveDirectory, newFileName);

                using (FileStream fileStream = new FileStream(Path.Combine(saveDirectory!, newFileName), FileMode.Create, FileAccess.Write))
                {
                    stream.Position = 0;
                    stream.CopyToAsync(fileStream);
                }
            }
        }

        private static string SetFileNameOccurance(string path, string fileName)
        {
            int occurrence = 1;
            string fileExtension = Path.GetExtension(fileName);
            string newFileName = fileName;
            while (File.Exists(Path.Combine(path, newFileName)))
            {
                occurrence++;
                newFileName = Path.GetFileNameWithoutExtension(fileName) +
                    " (" + occurrence + ")" +
                    fileExtension;
            }

            return newFileName;
        }

        private static string GetFileName(string url, string path ) {
            string fileName = Path.GetFileName( path );
            if (string.IsNullOrEmpty(fileName)) {
                Uri uri = new Uri(url);
                fileName = System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath);
            }
            if (string.IsNullOrEmpty( fileName )) {
                fileName = "untitled";
            }

            return Path.GetFileNameWithoutExtension(fileName);
        }

        private static string GetFileExtension(string url, string path, Stream stream) {
            string fileExtension = Path.GetExtension( path );
            if (string.IsNullOrEmpty(fileExtension)) {
                fileExtension = Path.GetExtension( url );
                if (fileExtension[0] == '-' || fileExtension[0] == '.')
                {
                    fileExtension = "";
                }
            }
            if(string.IsNullOrEmpty(fileExtension)) {
                using(MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);

                    memoryStream.Position = 0;

                    byte[] buffer = new byte[256];
                    memoryStream.Read(buffer, 0, buffer.Length);
                    fileExtension = FileExtensions.GetFileExtension(buffer);
                }
            }

            return fileExtension;
        }
    }
}
