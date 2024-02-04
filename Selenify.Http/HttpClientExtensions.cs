using Selenify.Http.StreamUtilities;
using System.Net.Http.Headers;
using static Selenify.Common.Console.Console;
using Selenify.Http.Models;

namespace Selenify.Http
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

        public static async Task<string> DownloadAsync(this HttpClient client, string url, FileStream file)
        {
            string filePath;
            using (DownloadFileStream fileStream = await DownloadFileStream.CreateAsync(client, url, file))
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

        public static async Task<string> DownloadWithProgressBarAsync(this HttpClient client, string url, FileStream file)
        {
            string filePath;
            using (DownloadFileStream fileStream = await DownloadFileStream.CreateAsync(client, url, file))
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

        public static void ResetBasicAuthHeader(this HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
