﻿using OpenQA.Selenium.DevTools.V116.DOM;
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
            UI.Clear();
            System.Console.WriteLine(s);
        }

        public static void Write (string s, params object[] args)
        {
            UI.Clear();
            System.Console.Write(s, args);
        }

        public static class UI
        {
            private static List<string> Lines = new List<string>();

            public static void Clear()
            {
                ClearOutputBelow(0);
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
                int startIndex = GetStartIndex(lineIndex);
                
                ExtendLinesIfIndexOutOfRange(lineIndex);

                ClearOutputBelow(startIndex);

                string[] strings = text.Split("\n");
                Lines.RemoveAt(lineIndex);
                Lines.InsertRange(lineIndex, strings);

                for (int i = startIndex; i < Lines.Count; i++)
                {
                    System.Console.WriteLine(Lines[i]);
                }
            }

            private static int GetStartIndex(int lineIndex)
            {
                if (lineIndex < Lines.Count)
                {
                    return lineIndex;
                }

                return Lines.Count;
            }

            public static void UpdateLineFrom(int lineIndex, params string[] texts)
            {
                int startIndex = GetStartIndex(lineIndex);

                ExtendLinesIfIndexOutOfRange(lineIndex);

                ClearOutputBelow(startIndex);

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

            private static void ExtendLinesIfIndexOutOfRange(int lineIndex)
            {
                while (lineIndex >= Lines.Count)
                {
                    Lines.Add("");
                }
            }



            private static int CalculateLineHeightUntilPosition(int lineIndex)
            {
                int totalHeight = 0;
                for (int i = lineIndex; i < Lines.Count; i ++)
                {
                    totalHeight += (int)Math.Ceiling(
                        (double)Lines[i].Length / System.Console.BufferWidth
                    );
                }

                return totalHeight;
            }

            public static void Stop()
            {
                Lines.Clear();
            }
        }
    }
}
