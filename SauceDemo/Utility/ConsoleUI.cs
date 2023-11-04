using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Utility {
    public static class ConsoleUI {
        private static int startLine = 0;
        private static int endLine = 0;

        public static void Write( string text ) {
            Clear();

            startLine = Console.CursorTop;
            Console.WriteLine( text );
            endLine = Console.CursorTop;
        }

        public static void Clear() {
            if (startLine < endLine) {
                Console.SetCursorPosition( 0, startLine );

                for (int i = startLine; i < endLine; i++) {
                    Console.Write( new string( ' ', Console.BufferWidth ) );
                    if (i < endLine - 1) {
                        Console.WriteLine();
                    }
                }

                Console.SetCursorPosition( 0, startLine );
            }
        }

        public static void Stop() {
            startLine = 0;
            endLine = 0;
        }

        public static void Reset() {
            Clear();
            Stop();
        }
    }
}
