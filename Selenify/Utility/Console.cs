using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Utility
{
    static class Console
    {
        public static void WriteLine(string s)
        {
            if (UI.endLine > 0){
                UI.Reset();
            }
            System.Console.WriteLine(s);
        }

        public static void Write (string s, params object[] args)
        {
            if (UI.endLine > 0){
                UI.Reset();
            }
            System.Console.Write(s, args);
        }

        public static class UI
        {
            private static List<string> Lines = new List<string>();
            public static int endLine = 0;

            private static int CalculateUIHeight()
            {
                int height = 0;

                foreach(string line in Lines)
                {
                    height += (int)Math.Ceiling(
                        (double)line.Length / System.Console.BufferWidth
                    );
                }

                return height;
            }

            public static void Clear()
            {
                int UIHeight = CalculateUIHeight();

                System.Console.SetCursorPosition(
                    0,
                    System.Console.CursorTop - UIHeight
                );

                for (int i = 0; i < UIHeight; i++)
                {
                    System.Console.WriteLine(
                        new string(' ', System.Console.BufferWidth)
                        );
                }

                System.Console.SetCursorPosition(
                    0,
                    System.Console.CursorTop - UIHeight
                );

                Lines.Clear();
            }

            public static void WriteLines(params string[] texts)
            {
                Clear();

                foreach (string text in texts)
                {
                    var lineTexts = text.Split('\n');
                    Lines.AddRange(lineTexts);

                    System.Console.WriteLine(text);
                }
            }

            public static void WriteLine(string text)
            {
                Clear();

                string[] texts = text.Split("\n");
                Lines.AddRange(texts);
                System.Console.WriteLine(text);
            }

            public static void AppendLine(string text)
            {
                string[] texts = text.Split("\n");
                Lines.AddRange(texts);
                System.Console.WriteLine(text);
            }

            public static void UpdateLine(int lineIndex, string text)
            {
                EnsureLinesExist(lineIndex);
                ClearOutputBelow(lineIndex);

                string[] strings = text.Split("\n");
                Lines.RemoveAt(lineIndex);
                Lines.InsertRange(lineIndex, strings);

                for (int i = lineIndex; i < Lines.Count; i++)
                {
                    System.Console.WriteLine(Lines[i]);
                }
            }

            private static void ClearOutputBelow(int lineIndex)
            {
                int height = CalculateLineHeightUntilPosition(lineIndex);
                
                System.Console.SetCursorPosition(
                0, System.Console.CursorTop - height);

                for (int i = 0; i < height; i++)
                {
                    System.Console.WriteLine(
                        new string(' ', System.Console.BufferWidth));
                }

                System.Console.SetCursorPosition(
                    0, System.Console.CursorTop - height);
            }

            private static void EnsureLinesExist(int lineIndex)
            {
                bool uiIsEmpty = Lines.Count == 0;
                while (lineIndex >= Lines.Count)
                {
                    Lines.Add("");
                }

                if (uiIsEmpty)
                {
                    WriteLines(Lines.ToArray());
                }
            }

            private static int CalculateLineHeightUntilPosition(int lineIndex)
            {
                List<string> reversedLines = new List<string>(Lines);
                reversedLines.Reverse();

                int totalHeight = 0;
                for (int i = 0; i < lineIndex; i ++)
                {
                    totalHeight += (int)Math.Ceiling(
                        (double)reversedLines[i].Length / System.Console.BufferWidth
                    );
                }

                return totalHeight;
            }

            public static void Stop()
            {
                Lines.Clear();
            }

            public static void Reset()
            {
                Stop();
                Clear();
            }
        }
    }
}
