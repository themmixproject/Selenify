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
            FileStream = new FileStream(
                Path,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.None
            );
        }

        public void Dispose()
        {
            File.Delete(Path);
            FileStream?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
