using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Console
{
    public partial class Console
    {
        public class ProgressBar : IProgress<float>, IDisposable
        {
            bool hasFinished = false;

            private const int blockCount = 10;
            private string Prefix { get; set; }

            private readonly TimeSpan interval = TimeSpan.FromSeconds(1.0 / 30);

            private readonly Timer timer;

            private float currentProgress = 0;
            private string currentText = string.Empty;

            private bool disposed = false;

            public ProgressBar(string prefix)
            {
                Prefix = prefix;

                timer = new Timer(TimerHandler!);

                if (!System.Console.IsOutputRedirected)
                {
                    ResetTimer();
                }
            }

            public ProgressBar()
            {
                Prefix = string.Empty;

                timer = new Timer(TimerHandler!);

                if (!System.Console.IsOutputRedirected)
                {
                    ResetTimer();
                }
            }

            public void Report(float value)
            {
                // Make sure the value is in [0..1] range
                value = Math.Max(0, Math.Min(1, value));
                Interlocked.Exchange(ref currentProgress, value);
            }

            private void TimerHandler(object state)
            {
                lock (timer)
                {
                    if (disposed)
                    {
                        return;
                    }

                    int progressBlockCount = (int)(currentProgress * blockCount);
                    int percent = (int)(currentProgress * 100);
                    string text = string.Format("[{0}{1}] {2}%",
                        new string('#', progressBlockCount), new string('-', blockCount - progressBlockCount),
                        percent);
                    UpdateText(text);

                    ResetTimer();
                }
            }

            private void UpdateText(string text)
            {
                int commonPrefixLength = 0;
                int commonLength = Math.Min(currentText.Length, text.Length);
                while (commonPrefixLength < commonLength && text[commonPrefixLength] == currentText[commonPrefixLength])
                {
                    commonPrefixLength++;
                }

                // Backtrack to the first differing chracter
                StringBuilder outputBuilder = new StringBuilder();
                outputBuilder.Append('\b', currentText.Length - commonPrefixLength);

                // Output new Suffix
                outputBuilder.Append(text.Substring(commonPrefixLength));

                // If the new text is shorter than the old one: delete overlapping characters
                int overlapCount = currentText.Length - text.Length;
                if (overlapCount > 0)
                {
                    outputBuilder.Append(' ', overlapCount);
                    outputBuilder.Append('\b', overlapCount);
                }

                UI.WriteLine(text);
                currentText = text;
            }

            private void ResetTimer()
            {
                timer.Change(interval, TimeSpan.FromMilliseconds(-1));
            }

            public void Dispose()
            {
                lock (timer)
                {
                    disposed = true;
                    Report(1);
                }
            }
        }
    }
}
