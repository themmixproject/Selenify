using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Utility
{
    public static partial class Console
    {
        public static void WriteLine(string s)
        {
            UI.Clear();
            System.Console.WriteLine(s);
        }

        public static void Write(string s, params object[] args)
        {
            UI.Clear();
            System.Console.Write(s, args);
        }
    }
}
