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
                System.Console.WriteLine(line);
            }
            endLine = System.Console.CursorTop;
        }

        public static void WriteLine(string text)
        {
            Clear();
            var newLines = text.Split('\n');
            lines = new List<string>(newLines);
            foreach (var line in newLines)
            {
                System.Console.WriteLine(line);
            }
            endLine = System.Console.CursorTop;
        }

        public static void AppendLine(string text)
        {
            var newLines = text.Split('\n');
            lines.AddRange(newLines);
            foreach (var line in newLines)
            {
                System.Console.WriteLine(line);
            }
            endLine = System.Console.CursorTop;
        }

        public static void UpdateLine(int index, string text)
        {
            while (index >= lines.Count)
            {
                lines.Add("");
            }

            var newLines = text.Split('\n');
            lines.RemoveAt(index);
            lines.InsertRange(index, newLines);

            System.Console.SetCursorPosition(0, startLine + index);

            System.Console.Write(new string(' ', System.Console.BufferWidth));
            System.Console.SetCursorPosition(0, startLine + index);
            System.Console.Write(lines[index]);

            endLine = startLine + lines.Count;

            System.Console.SetCursorPosition(0, endLine);
        }

        public static void Clear()
        {
            if (startLine < endLine)
            {
                System.Console.SetCursorPosition(0, startLine);

                for (int i = startLine; i < endLine; i++)
                {
                    System.Console.Write(new string(' ', System.Console.BufferWidth));
                    if (i < endLine - 1)
                    {
                        System.Console.WriteLine();
                    }
                }

                System.Console.SetCursorPosition(0, startLine);
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
