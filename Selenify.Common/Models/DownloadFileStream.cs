using Selenify.Common.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Models
{
    public class DownloadFileStream : IDisposable
    {
        public HttpResponseMessage? Response { get; private set; }
        public Stream? Stream { get; set; }
        public FileStream? File { get; private set; }

        public static async Task<DownloadFileStream> CreateAsync(HttpClient client, string fileUrl, string path)
        {
            var downloadFileStream = new DownloadFileStream ();

            downloadFileStream.Response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
            downloadFileStream.Stream = await downloadFileStream.Response.Content.ReadAsStreamAsync();

            byte[] buffer = new byte[8];
            int bytesRead = await downloadFileStream.Stream.ReadAsync(buffer, 0, buffer.Length);
            string savePath = GetFilePathForDownload(fileUrl, path, buffer);

            downloadFileStream.File = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await downloadFileStream.File.WriteAsync(buffer, 0, bytesRead);

            return downloadFileStream;
        }

        private static string GetFilePathForDownload(string fileUrl, string path, byte[] bytes)
        {
            string urlWithoutQuery = new Uri(fileUrl).GetLeftPart(UriPartial.Path);
            string fileName = Common.File.FileHelper.GetFileNameFromUrlOrDefault(urlWithoutQuery);

            fileName += Common.File.FileHelper.GetFileExtensionFromUrlOrByteArray(urlWithoutQuery, bytes);

            string saveDirectory = Path.GetDirectoryName(path)!;
            fileName = Common.File.FileHelper.IncrementFileNameIfDuplicate(saveDirectory, fileName);

            return Path.Combine(saveDirectory!, fileName);
        }

        public void Dispose()
        {
            Response?.Dispose();
            Stream?.Dispose();
            File?.Dispose();
        }


    }
}
