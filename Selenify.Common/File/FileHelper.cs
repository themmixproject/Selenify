using FileSignatures;
using Selenify.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.File
{
    public static class FileHelper
    {
        public static void CreateFileIfNotExists(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path);
            }
        }

        public static string IncrementFileNameIfDuplicate(string path, string fileName)
        {
            int occurrence = 1;
            string fileExtension = Path.GetExtension(fileName);
            string newFileName = fileName;
            while (System.IO.File.Exists(Path.Combine(path, newFileName)))
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

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "untitled";
            }

            return fileName;
        }

        public static string GetFileExtensionFromUrlOrByteArray(string url, byte[] bytes)
        {
            string fileExtension = Path.GetExtension(url);
            if (fileExtension[0] == '-' || fileExtension[0] == '.')
            {
                fileExtension = "";
            }
            if (string.IsNullOrEmpty(fileExtension))
            {
                fileExtension = GetExtensionFromByteArray(bytes);
            }


            return fileExtension;
        }

        public static string GetExtensionFromByteArray(byte[] bytes)
        {
            string? fileExtension = GetFileExtensionFromByteBuffer(bytes);
            if (string.IsNullOrEmpty(fileExtension))
            {
                FileFormatInspector inspector = new FileFormatInspector();

                MemoryStream byteStream = new MemoryStream(bytes);
                fileExtension = "." + inspector.DetermineFileFormat(byteStream)!.Extension;
            }

            return fileExtension;
        }

        public static string? GetFileExtensionFromByteBuffer(byte[] buffer)
        {
            string? extension = null;
            if (buffer.Length < 8)
            {
                return extension;
            }

            if (buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF && buffer[3] == 0xE0)
            {
                extension = ".jfif";
            }
            else if (buffer[0] == 0x52 && buffer[1] == 0x4D && buffer[2] == 0x54 && buffer[3] == 0x41)
            {
                extension = ".webm";
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
            else if (buffer[0] == 0x6D && buffer[1] == 0x6F && buffer[2] == 0x6F && buffer[3] == 0x76 ||
                buffer[0] == 0x71 && buffer[1] == 0x74 && buffer[2] == 20 && buffer[3] == 20)
            {
                extension = ".mov";
            }

            return extension;
        }
    }
}
