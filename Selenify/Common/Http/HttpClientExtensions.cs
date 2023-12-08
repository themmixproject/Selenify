using Selenify.Common.Extensions;
using Selenify.Common.Utility;
using Selenify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Http
{
    public static class HttpClientExtensions
    {
        public static async Task DownloadAsync(HttpClient client, string url, string path)
        {
            using (DownloadFileStream fileStream = await DownloadFileStream.CreateAsync(client, url, path))
            {
                await fileStream.Stream.CopyToAsync(fileStream.File);
            }
        }

        public static async Task DownloadAsync(string url, string path)
        {
            await DownloadAsync(HttpClientManager.Client, url, path);
        }

        public static async Task DownloadWithProgressBarAsync(HttpClient client, string url, string path)
        {
            var progressBar = new ConsoleProgressBar("Downloading File . . . ");
            using (DownloadFileStream fileStream = await DownloadFileStream.CreateAsync(client, url, path))
            {
                long? responseContentLength = fileStream.Response.Content.Headers.ContentLength;

                var relativeProgress = new Progress<long>(totalbytes => progressBar.Report((float)totalbytes / responseContentLength!.Value));
                await fileStream.Stream.CopyToAsync(fileStream.File, 81920, relativeProgress, new CancellationToken());
            }

            progressBar.Report(1);
        }

        public static async Task DownloadWithProgressBarAsync(string url, string path)
        {
            await DownloadWithProgressBarAsync(HttpClientManager.Client, url, path);
        }
    }
}
