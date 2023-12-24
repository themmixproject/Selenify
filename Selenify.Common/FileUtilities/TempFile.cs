using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.FileUtilities
{
    public class TempFile : IDisposable
    {
        public string Path { get; private set; }
        public FileStream FileStream { get; private set; }
        public TempFile()
        {
            Path = System.IO.Path.GetTempFileName();
            FileStream = System.IO.File.Create(Path);
        }

        public void Dispose()
        {
            System.IO.File.Delete(Path);
            FileStream?.Dispose();
        }
    }
}
