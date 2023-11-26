using Selenify.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Helpers
{
    public static class FileHelper
    {
        public static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void CreateFileIfNotExists(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

        public static string IncrementFileNameIfDuplicate(string path, string fileName)
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

        public static string GetFileNameFromUrlOrDefault(string url)
        {
            string fileName = string.Empty;
            Uri uri = new Uri(url);
            fileName = Path.GetFileNameWithoutExtension(uri.LocalPath);

            if(string.IsNullOrEmpty(fileName) )
            {
                fileName = "untitled";
            }

            return fileName;
        }

        public static string GetFileExtensionFromUrlOrStream(string fileSource, Stream stream)
        {
            string fileExtension = Path.GetExtension(fileSource);
            if (fileExtension[0] == '-' || fileExtension[0] == '.')
            {
                fileExtension = "";
            }
            if (string.IsNullOrEmpty(fileExtension))
            {
                fileExtension = GetFileExtensionFromStream(stream);
            }

            return fileExtension;
        }

        public static string GetFileExtensionFromStream(Stream stream)
        {
            string fileExtension;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);

                memoryStream.Position = 0;

                byte[] buffer = new byte[256];
                memoryStream.Read(buffer, 0, buffer.Length);
                fileExtension = GetFileExtensionFromByteBuffer(buffer);
            }

            return fileExtension;
        }

        public static string GetFileExtensionFromByteBuffer(byte[] buffer)
        {
            string extension = ".txt";
            if (buffer.Length >= 2)
            {
                if (buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF && buffer[3] == 0xE0)
                {
                    extension = ".jfif";
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xD8)
                {
                    extension = ".jpg";
                }
                else if (buffer[0] == 0x89 && buffer[1] == 0x50)
                {
                    extension = ".png";
                }
                else if (buffer[0] == 0x47 && buffer[1] == 0x49)
                {
                    extension = ".gif";
                }
                else if (buffer[0] == 0x42 && buffer[1] == 0x4D)
                {
                    extension = ".bmp";
                }
                else if (buffer[0] == 0x30 && buffer[1] == 0x26 && buffer[2] == 0xB2 && buffer[3] == 0x75)
                {
                    extension = ".wmv";
                }
                else if (buffer[0] == 0x50 && buffer[1] == 0x4B)
                {
                    extension = ".zip";
                }
                else if (buffer[0] == 0x66 && buffer[1] == 0x74 && buffer[2] == 0x79 && buffer[3] == 0x70)
                {
                    extension = ".mp4";
                }
                else if (buffer[0] == 0x6D && buffer[1] == 0x6F && buffer[2] == 0x6F && buffer[3] == 0x76)
                {
                    extension = ".mov";
                }
                else if (buffer[0] == 0x71 && buffer[1] == 0x74 && buffer[2] == 0x20 && buffer[3] == 0x20)
                {
                    extension = ".mov";
                }
            }

            return extension;
        }
    }
}
