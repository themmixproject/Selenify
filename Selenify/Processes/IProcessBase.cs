using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Processes
{
    public interface IProcessBase
    {
        public string ProcessName { get; set; }
        public void Run();
    }
}
