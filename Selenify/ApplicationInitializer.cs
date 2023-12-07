using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Selenify
{
    public static class ApplicationInitializer
    {
        public static void StartUp()
        {
            string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            string tempDirectoryPath = Path.Combine(assemblyLocation, "tmp");

            Common.Helpers.FileHelper.CreateDirectoryIfNotExists(tempDirectoryPath);
        }
    }
}
