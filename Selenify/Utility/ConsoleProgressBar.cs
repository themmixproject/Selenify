using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Utility
{
    public class ConsoleProgressBar : IProgress<float>
    {
        private int barSize = System.Console.BufferWidth;
        private readonly char progressCharacter = '=';

        public void Report(float value)
        {
            string percentage = Math.Round(value * 100, 2).ToString().PadLeft(4) + "%";
            int blockCount = 20;
            int progressBlockCount = (int)(blockCount * value);
            string text = "[" + new string('#', progressBlockCount) +
                new string('-', blockCount - progressBlockCount) + "] " + percentage;

            Console.UI.UpdateLine(1, text);

        }
    }
}
