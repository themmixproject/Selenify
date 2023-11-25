using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Utility
{
    public class ConsoleProgressBar : IProgress<float>
    {
        private int barSize = System.Console.BufferWidth;
        private readonly char progressCharacter = '=';
        private int lineToUpdate = Console.UI.LineCount;

        private string Prefix {  get; set; }

        public ConsoleProgressBar(string prefix)
        {
            Prefix = prefix;
        }

        public ConsoleProgressBar() {
            Prefix = string.Empty;
        }

        public void Report(float value)
        {
            string percentage = Math.Round(value * 100, 2).ToString().PadLeft(5) + "%";
            int blockCount = 20;
            int progressBlockCount = (int)(blockCount * value);
            string text = "[" + new string('#', progressBlockCount) +
                new string('-', blockCount - progressBlockCount) + "]" + percentage;

            System.Console.Write("\r" + Prefix + text);

            if (value == 1)
                System.Console.Write("\r" + new string(' ', System.Console.BufferWidth) + "\r");
            }
    }
}
