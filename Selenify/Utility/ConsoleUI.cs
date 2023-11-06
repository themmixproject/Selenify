using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Utility
{
    public static class ConsoleUI
    {
        private static int startLine = 0;
        private static int endLine = 0;
        private static List<string> lines = new List<string>();

        public static void WriteLines(params string[] texts)
        {
            Clear();
            lines = texts.SelectMany(text => text.Split('\n')).ToList();
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            endLine = Console.CursorTop;
        }

        public static void WriteLine(string text)
        {
            Clear();
            var newLines = text.Split('\n');
            lines = new List<string>(newLines);
            foreach (var line in newLines)
            {
                Console.WriteLine(line);
            }
            endLine = Console.CursorTop;
        }

        public static void AppendLine(string text)
        {
            var newLines = text.Split('\n');
            lines.AddRange(newLines);
            foreach (var line in newLines)
            {
                Console.WriteLine(line);
            }
            endLine = Console.CursorTop;
        }

        public static void UpdateLine(int index, string text)
        {
            var newLines = text.Split('\n');
            lines.RemoveAt(index);
            lines.InsertRange(index, newLines);

            Console.SetCursorPosition(0, startLine + index);

            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, startLine + index);
            Console.Write(lines[index]);

            Console.SetCursorPosition(0, endLine);
        }

        public static void Clear()
        {
            if (startLine < endLine)
            {
                Console.SetCursorPosition(0, startLine);

                for (int i = startLine; i < endLine; i++)
                {
                    Console.Write(new string(' ', Console.BufferWidth));
                    if (i < endLine - 1)
                    {
                        Console.WriteLine();
                    }
                }

                Console.SetCursorPosition(0, startLine);
            }
            lines.Clear();
            startLine = 0;
            endLine = 0;
        }

        public static void Stop()
        {
            startLine = 0;
            endLine = 0;
        }

        public static void Reset()
        {
            Clear();
            Stop();
        }
    }
}
