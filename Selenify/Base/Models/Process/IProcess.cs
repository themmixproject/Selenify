using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Base.Models.Process
{
    public interface IProcess
    {
        public string ProcessName { get; set; }
        public void Run();
    }
}
