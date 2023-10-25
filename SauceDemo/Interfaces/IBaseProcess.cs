using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Interfaces
{
    internal interface IBaseProcess
    {
        public string ProcessName { get; set; }

        public void Run();
    }
}
