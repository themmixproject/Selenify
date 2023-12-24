using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.IO;
using Selenify.Common.FileUtilities;

namespace Selenify.Common.Wget
{
    public static class Wget
    {
        public static string ExecuteCommand(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "wget.exe";
            process.StartInfo.Arguments = command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }

        public static string DownloadWebpage(string url, string dir)
        {
            return ExecuteCommand( GetBaseCommandString(url, dir) );
        }

        public static string DownloadWebPageWithBasicAuth(string url, string dir, string username, string password)
        {
            return ExecuteCommand("--user=\"" + username + " --password=\"" + password + "\" " + GetBaseCommandString(url, dir));
        }

        public static string DownloadWebPageWithCookieContainer(string url, string dir, CookieContainer cookieContainer)
        {
            using (TemporaryCookieFile cookieFile = new TemporaryCookieFile(cookieContainer))
            {
                return ExecuteCommand("--load-cookies \"" + cookieFile.CookieFile + "\" " + GetBaseCommandString(url, dir));
            }
        }

        private static string GetBaseCommandString(string url, string dir)
        {
            return "-E -H -k -K -p -P \"" + dir + "\" " + url;
        }

        private class TemporaryCookieFile : IDisposable
        {
            public TempFile CookieFile {  get; set; }
            public TemporaryCookieFile(CookieContainer cookieContainer)
            {
                CookieFile = new TempFile();
                using (var writer = new StreamWriter(CookieFile.FileStream))
                {
                    foreach(Cookie cookie in cookieContainer.GetAllCookies())
                    {
                        writer.WriteLine(
                            "{0}\tTRUE\t{1}\t{2}\t{3}\t{4}\t{5}",
                            cookie.Domain,
                            cookie.Path,
                            cookie.Secure ? "TRUE" : "FALSE",
                            cookie.Expires == DateTime.MinValue ? "0" : ((DateTimeOffset)cookie.Expires).ToUnixTimeSeconds().ToString(),
                            cookie.Name,
                            cookie.Value
                        );
                    }
                }
            }

            public void Dispose()
            {
                CookieFile.Dispose();
            }
        }
    }
}
