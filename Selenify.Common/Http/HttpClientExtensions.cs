using Selenify.Common.Extensions;
using Selenify.Common.Models;
using Selenify.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            using (Utility.Console.ProgressBar progressBar = new Utility.Console.ProgressBar("Downloading File . . ."))
            {
                using (DownloadFileStream fileStream = await DownloadFileStream.CreateAsync(client, url, path))
                {
                    long? responseContentLength = fileStream.Response!.Content.Headers.ContentLength;

                    var relativeProgress = new Progress<long>(totalBytes =>
                    {
                        float percentage = (float)totalBytes / responseContentLength!.Value;
                        progressBar.Report(percentage);
                        if (percentage == 1)
                        {
                            cancellationTokenSource.Cancel();
                        }
                    });

                    await fileStream.Stream!.CopyToAsync(fileStream.File!, 81920, relativeProgress, cancellationToken);
                    filePath = fileStream.File!.Name;
                }

                progressBar.Report(1);
                cancellationTokenSource.Cancel();
                await Task.Delay(100);
            }

            return filePath;
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
