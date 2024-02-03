using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace Selenify.Common.Tesseract
{
    public static class TesseractHelper
    {
        public static string GetTextFromImage(string imagePath)
        {
            using (TesseractEngine engine = new TesseractEngine(@"./Tesseract/tessdata", "eng", EngineMode.Default, "./Tesseract/tesseractconfig.txt"))
            {
                using (var image = Pix.LoadFromFile(imagePath))
                {
                    using (var page = engine.Process(image))
                    {
                        return page.GetText();
                    }
                }
            }
        }
    }
}
