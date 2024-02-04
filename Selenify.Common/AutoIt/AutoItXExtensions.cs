using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoIt;
using static System.Net.Mime.MediaTypeNames;

namespace Selenify.Common.AutoIt
{
    public static class AutoItXExtensions
    {
        public static string PrepareSendText(string sendText)
        {
            StringBuilder preparedText = new StringBuilder();

            for (int i = 0; i < sendText.Length; i++)
            {
                // If the character is an uppercase letter
                if (char.IsUpper(sendText[i]))
                {
                    preparedText.Append("{SHIFTDOWN}" + sendText[i] + "{SHIFTUP}");
                }
                // If the character is the start of a special key (in brackets)
                else if (sendText[i] == '{')
                {
                    int endBracketIndex = sendText.IndexOf('}', i);
                    if (endBracketIndex != -1)
                    {
                        string specialKey = sendText.Substring(i, endBracketIndex - i + 1);
                        preparedText.Append(specialKey);
                        i = endBracketIndex; // Skip the processed special key
                    }
                }
                // If the character is a lowercase letter or other character
                else
                {
                    preparedText.Append(sendText[i]);
                }
            }

            return preparedText.ToString();
        }

    }
}
