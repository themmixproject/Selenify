using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Console
{
    public partial class Console
    {
        public static class UI
        {
            private static List<string> Lines = new List<string>();
            private static object _lock = new object();

            public static int LineCount { get { return Lines.Count; } }

            public static void Clear()
            {
                lock (_lock)
                {
                    ClearOutputBelow(0);
                    Lines.Clear();
                }
            }

            public static void WriteLines(params string[] texts)
            {
                lock (_lock)
                {
                    Clear();

                    foreach (string text in texts)
                    {
                        AppendLine(text);
                    }
                }
            }

            public static void WriteLine(string text)
            {
                lock (_lock)
                {
                    Clear();
                    AppendLine(text);
                }
            }

            public static void AppendLine(string text)
            {
                lock (_lock)
                {
                    string[] texts = text.Split("\n");
                    Lines.AddRange(texts);

                    System.Console.WriteLine(text);
                }
            }

            public static void UpdateLine(int lineIndex, string text)
            {
                lock (_lock)
                {
                    int startIndex = Math.Min(lineIndex, Lines.Count);

                    ClearOutputBelow(startIndex);

                    ExtendLinesIfIndexOutOfRange(lineIndex);


                    string[] strings = text.Split("\n");
                    Lines.RemoveAt(lineIndex);
                    Lines.InsertRange(lineIndex, strings);

                    for (int i = startIndex; i < Lines.Count; i++)
                    {
                        System.Console.WriteLine(Lines[i]);
                    }
                }
            }

            public static void UpdateLineFrom(int lineIndex, params string[] texts)
            {
                lock (_lock)
                {
                    int startIndex = Math.Min(lineIndex, Lines.Count);

                    ClearOutputBelow(startIndex);

                    ExtendLinesIfIndexOutOfRange(lineIndex);


                    for (int i = startIndex; i < Lines.Count; i++)
                    {
                        Lines[i] = "";
                    }

                    Lines.RemoveRange(lineIndex, Lines.Count - lineIndex);

                    foreach (string text in texts)
                    {
                        var lineTexts = text.Split('\n');
                        Lines.AddRange(lineTexts);
                    }

                    for (int i = startIndex; i < Lines.Count; i++)
                    {
                        System.Console.WriteLine(Lines[i]);
                    }
                }
            }

            private static void ClearOutputBelow(int lineIndex)
            {
                int height = 0;
                lock (_lock)
                {
                    height = CalculateLineHeightUntilPosition(lineIndex);
                }

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

            private static void ExtendLinesIfIndexOutOfRange(int lineIndex)
            {
                lock (_lock)
                {
                    while (lineIndex >= Lines.Count)
                    {
                        Lines.Add("");
                    }
                }
            }

            private static int CalculateLineHeightUntilPosition(int lineIndex)
            {
                int totalHeight = 0;
                for (int i = lineIndex; i < Lines.Count; i++)
                {
                    totalHeight += Math.Max(1, (int)Math.Ceiling((double)Lines[i].Length / System.Console.BufferWidth));
                }

                return totalHeight;
            }

            public static void Stop()
            {
                lock (_lock)
                {
                    Lines.Clear();
                }
            }
        }
    }
}
