using Selenify.Common.Extensions;
using Selenify.Common.Models;
using Selenify.Common.Utility;
using Selenify.Common.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static Selenify.Common.Console.Console;
using System.Threading;

namespace Selenify.Common.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<string> DownloadAsync(this HttpClient client, string url, string path)
        {
            string filePath;
            using (DownloadFileStream fileStream = await DownloadFileStream.CreateAsync(client, url, path))
            {
                await fileStream.Stream!.CopyToAsync(fileStream.File!);
                filePath = fileStream.File!.Name;
            }

            return filePath;
        }

        public static async Task<string> DownloadWithProgressBarAsync(this HttpClient client, string url, string path)
        {
            string filePath;
            using (DownloadFileStream fileStream = await DownloadFileStream.CreateAsync(client, url, path))
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = cancellationTokenSource.Token;
                
                string progressBarPrefix = "Downloading " + Path.GetFileName(fileStream.File!.Name) + " ";
                using (ProgressBar progressBar = new ProgressBar(progressBarPrefix))
                {
                    filePath = await CopyStreamWithProgressBarAsync(fileStream, progressBar, cancellationToken);
                }

                cancellationTokenSource.Cancel();
            }

            return filePath;
        }

        private static async Task<string> CopyStreamWithProgressBarAsync(DownloadFileStream fileStream, ProgressBar progressBar, CancellationToken cancellationToken)
        {
            long? responseContentLength = fileStream.Response!.Content.Headers.ContentLength;

            var relativeProgress = new Progress<long>(totalBytes =>
            {
                float percentage = (float)totalBytes / responseContentLength!.Value;
                progressBar.Report(percentage);
            });

            await fileStream.Stream!.CopyToAsync(fileStream.File!, 81920, relativeProgress, cancellationToken);
            return fileStream.File!.Name;
        }

        public static async Task<string> DownloadToTempFolder(this HttpClient client, string url)
        {
            return await client.DownloadAsync(url, "tmp\\");
        }

        public static async Task<string> DownloadToTempFolder(this HttpClient client, string url, string imageName)
        {
            return await client.DownloadAsync(url, "tmp\\" + imageName);
        }

        public static void SetBasicAuthHeader(this HttpClient client, string username, string password)
        {
            byte[] asciiString = System.Text.Encoding.ASCII.GetBytes(username + ":" + password);
            string authString = Convert.ToBase64String(asciiString);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
        }
    }
}
