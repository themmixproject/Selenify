using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Utility
{
    public static class FileManager
    {
        public static void CreateDirectoryIfNotExists(string path)
        {
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void CreateFileIfNotExists(string path)
        {
            if(!File.Exists(path))
            {
                File.Create(path);
            }
        }
    }
}
