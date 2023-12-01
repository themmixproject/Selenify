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
using System.Xml.Linq;
using System.Threading;
using Selenify.Models;

namespace Selenify.Common.Utility
{
    public static class DownloadManager
    {
        public static async Task DownloadAsync(HttpClient client,  string url, string path)
        {
            using (DownloadFileStream fileStream = await DownloadFileStream.CreateAsync(client, url, path))
            {
                await fileStream.Stream.CopyToAsync(fileStream.File);
            }
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
    }
}
