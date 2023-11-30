﻿using Selenify.Common.Helpers;
using Selenify.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Models
{
    public class DownloadFileStream : IDisposable
    {
        public HttpResponseMessage Response { get; private set; }
        public Stream Stream { get; set; }
        public FileStream File { get; private set; }

        private DownloadFileStream () { }

        public static async Task<DownloadFileStream> CreateAsync(HttpClient client, string fileUrl, string path)
        {
            var downloadFileStream = new DownloadFileStream ();

            client.Timeout = TimeSpan.FromMinutes(5);
            downloadFileStream.Response = await client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
            downloadFileStream.Stream = await downloadFileStream.Response.Content.ReadAsStreamAsync();

            byte[] buffer = new byte[256];
            int bytesRead = await downloadFileStream.Stream.ReadAsync(buffer, 0, buffer.Length);
            string savePath = GetFilePathForDownload(fileUrl, path, buffer);

            downloadFileStream.File = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await downloadFileStream.File.WriteAsync(buffer, 0, bytesRead);

            return downloadFileStream;
        }

        private static string GetFilePathForDownload(string fileUrl, string path, byte[] bytes)
        {
            string urlWithoutQuery = new Uri(fileUrl).GetLeftPart(UriPartial.Path);
            string fileName = FileHelper.GetFileNameFromUrlOrDefault(urlWithoutQuery);


            fileName += FileHelper.GetFileExtensionFromUrlOrByteArry(urlWithoutQuery, bytes);

            string saveDirectory = Path.GetDirectoryName(path)!;
            fileName = FileHelper.IncrementFileNameIfDuplicate(saveDirectory, fileName);

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